//MIT License
//
//Copyright (c) 2018 Tom Blind
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using Fuse.Tasks.Internal;

//This needs to be explicitly defined for the compiler
namespace System.Runtime.CompilerServices
{
	public sealed class AsyncMethodBuilderAttribute : Attribute
	{
		public Type BuilderType { get; }

		public AsyncMethodBuilderAttribute(Type builderType)
		{
			BuilderType = builderType;
		}
	}
}

namespace Fuse.Tasks
{
	//Routines do triple duty as task-likes, task-builders, and awaiters in order to keep internal access/pooling easy
	public abstract class RoutineBase : INotifyCompletion
	{
		/// <summary> Enable stack tracing for debugging. Off by default due to performance implications. </summary>
		public static bool TracingEnabled { get; set; }

		/// <summary> The running instance id of the routine. If the routine is stopped, will return zero. </summary>
		public UInt64 Id { get { return id; } }

		/// <summary> Indicates if routine is stopped. </summary>
		public bool IsDead { get { return id == 0; } }

		/// <summary> Internal use only. Required for awaiter. </summary>
		public bool IsCompleted { get { return state == State.Finished; } }

#if DEBUG
		/// <summary> The current async/await stack trace for this routine </summary>
		public string StackTrace
		{
			get
			{
				string formatFrame(System.Diagnostics.StackFrame frame)
				{
					if (frame == null)
					{
						return "(unknown) at unknown:0:0";
					}
					return string.Format(
						"{0}.{1} at {2}:{3}:{4}",
						frame.GetMethod().DeclaringType,
						frame.GetMethod().Name,
						frame.GetFileName(),
						frame.GetFileLineNumber(),
						frame.GetFileColumnNumber()
					);
				}
				var routine = this;
				var stackTrace = formatFrame(routine.stackFrame);
				while (routine.parent != null)
				{
					routine = routine.parent;
					stackTrace += "\n" + formatFrame(routine.stackFrame);
				}
				return stackTrace;
			}
		}
#endif

		protected enum State
		{
			NotStarted,
			Running,
			Finished
		}

		protected interface IStateMachineRef
		{
			void MoveNext();
		}

		protected class StateMachineRef<T> : IStateMachineRef where T : IAsyncStateMachine
		{
			public T value;
			public void MoveNext() { value.MoveNext(); }
		}

		protected UInt64 id = 0; //Used to verify a routine is still the same instance and hasn't been recycled
		protected State state = State.NotStarted;
		protected bool stopChildrenOnStep = false; //Kill children when stepping. Used by WaitForAny
		protected IStateMachineRef stateMachine = null; //The generated state machine for the async method
		protected RoutineManager manager = null; //The manager to use for WaitForNextFrame
		protected RoutineBase parent = null; //Routine that spawned this one
		protected readonly List<RoutineBase> children = new List<RoutineBase>(); //Routines spawned by this one
		protected Action onFinish = null; //Continuation to call when async method is finished
		protected Action<Exception> onStop = null;
#if DEBUG
		protected System.Diagnostics.StackFrame stackFrame = null; //Track where the routine was created for debugging
#endif

		//Top-most routine currently being stepped
		protected static RoutineBase Current { get { return (steppingStack.Count > 0) ? steppingStack.Peek() : null; } }

		//State machines are pooled by keeping a wrapped class version. This is pointless in debug where the state
		//machines are generated as classes, but useful in release where they are structs.
		protected static readonly TypedPool<IStateMachineRef> stateMachinePool = new TypedPool<IStateMachineRef>();

		private static UInt64 nextId = 1; //Id generator. 64bits should be enough, right?

		//Tracks actively stepping routines
		private static readonly Stack<RoutineBase> steppingStack = new Stack<RoutineBase>();

		//Pools
		private static readonly TypedPool<RoutineBase> pool = new TypedPool<RoutineBase>();
		private static readonly TypedPool<IResumerBase> resumerPool = new TypedPool<IResumerBase>();

		/// <summary> Stop the routine. </summary>
		public void Stop()
		{
			Stop(null);
		}

		/// <summary> Internal use only. Executes to the next await or end of the async method. </summary>
		public void Step()
		{
			if (IsDead)
			{
				return;
			}

			//First step
			if (state == State.NotStarted)
			{
				state = State.Running;
			}

			//Step async method to the next await
			if (stateMachine != null)
			{
				//Stop children, but don't release them because their result might be needed
				if (stopChildrenOnStep)
				{
					foreach (var child in children)
					{
						child.Stop();
					}
				}

				var currentId = id;
				steppingStack.Push(this);
				stateMachine.MoveNext();
				Assert.IsTrue(steppingStack.Peek() == this);
				steppingStack.Pop();
				if (currentId != id)
				{
					return;
				}

				//Now we can release dead children back to the pool
				for (var i = 0; i < children.Count;)
				{
					var child = children[i];
					if (child.IsDead)
					{
						children.RemoveAt(i);
						Release(child);
					}
					else
					{
						++i;
					}
				}
			}

			//Routine was not an async method
			else
			{
				ReleaseChildren();
				state = State.Finished;
			}

			//All done: resume parent if needed
			if (state == State.Finished)
			{
				var _onFinish = onFinish;
				Stop();
				if (_onFinish != null)
				{
					_onFinish();
				}
			}
		}

		/// <summary> Internal use only. Receives continuation to call when async method finishes. </summary>
		public void OnCompleted(Action continuation)
		{
			onFinish = continuation;
		}

		/// <summary> Internal use only. Store the current stack frame for debuggging. </summary>
		[System.Diagnostics.Conditional("DEBUG")]
		public void Trace(int frame)
		{
#if DEBUG
			if (TracingEnabled)
			{
				stackFrame = new System.Diagnostics.StackFrame(frame + 1, true);
			}
#endif
		}

		/// <summary> Dump pooled objects to clear memory. </summary>
		public static void ClearPools()
		{
			stateMachinePool.Clear();
			pool.Clear();
			resumerPool.Clear();
		}

		public static void Report()
		{
			Debug.LogFormat("stateMachinePool = {0}", stateMachinePool.Report());
			Debug.LogFormat("pool = {0}", pool.Report());
			Debug.LogFormat("resumerPool = {0}", resumerPool.Report());
		}

		/// <summary> Get a routine from the pool. If yield is false routine will resume immediately from await. </summary>
		public static T Get<T>(bool yield) where T : RoutineBase, new()
		{
			var current = Current;
			if (current != null && current.IsDead)
			{
				throw new Exception("Routine is dead!");
			}
			var routine = pool.Get<T>();
			routine.Setup(yield, current);
			return routine;
		}

		/// <summary> Release routine back to pool. </summary>
		public static void Release(RoutineBase routine)
		{
			routine.Reset();
			pool.Release(routine);
		}

		/// <summary> Get a resumer from the pool. </summary>
		public static IResumer GetResumer()
		{
			return resumerPool.Get<Resumer>();
		}

		/// <summary> Get a resumer from the pool. </summary>
		public static IResumer<T> GetResumer<T>()
		{
			return resumerPool.Get<Resumer<T>>();
		}

		/// <summary> Release a resumer to the pool. </summary>
		public static void ReleaseResumer(IResumer resumer)
		{
			resumer.Reset();
			resumerPool.Release(resumer);
		}

		/// <summary> Release a resumer to the pool. </summary>
		public static void ReleaseResumer<T>(IResumer<T> resumer)
		{
			resumer.Reset();
			resumerPool.Release(resumer);
		}

		/// <summary>
		/// Do-nothing routine that resumes immediately. Good for quieting warning about async functions with no await
		/// statement.
		/// </summary>
		public static CTask Continue()
		{
			var continueRoutine = Get<CTask>(false);
			continueRoutine.SetResult();
			return continueRoutine;
		}

		/// <summary>
		/// Do-nothing routine that resumes immediately with the specified result. Good for quieting warning about async
		/// functions with no await statement.
		/// </summary>
		public static CTask<T> Continue<T>(T result)
		{
			var continueRoutine = Get<CTask<T>>(false);
			continueRoutine.SetResult(result);
			return continueRoutine;
		}

		/// <summary> Routine the yields until the next frame's update. Current routine must be managed. </summary>
		public static CTask WaitForNextFrame()
		{
			var nextFrameRoutine = Get<CTask>(true);
			nextFrameRoutine.Trace(1);
			var resumer = new LightResumer{routine = nextFrameRoutine, id = nextFrameRoutine.id};
			Current.manager.AddNextFrameResumer(ref resumer);
			return nextFrameRoutine;
		}

		/// <summary> Routine that yields until all routines in a collection complete. </summary>
		public static CTask WaitForAll(IEnumerable<RoutineBase> routines)
		{
			var allRoutine = Get<CTask>(true);
			allRoutine.Trace(1);
			var isCompleted = true;
			var currentId = allRoutine.id;
			foreach (var routine in routines)
			{
				routine.SetParent(allRoutine);
				routine.Start();
				if (allRoutine.id != currentId)
				{
					Assert.IsTrue(allRoutine.IsDead);
					return allRoutine;
				}
				if (!routine.IsCompleted)
				{
					routine.OnCompleted(allRoutine.StepAll);
					isCompleted = false;
				}
			}
			if (isCompleted)
			{
				allRoutine.StepAll();
			}
			return allRoutine;
		}

		/// <summary> Routine that yields until all routines in a collection complete. </summary>
		public static CTask WaitForAll(params RoutineBase[] routines)
		{
			var allRoutine = WaitForAll((IEnumerable<RoutineBase>)routines);
			allRoutine.Trace(1);
			return allRoutine;
		}

		/// <summary>
		/// Routine that yields until all routines in a collection complete. Returns array of results.
		/// </summary>
		public static CTask<T[]> WaitForAll<T>(IEnumerable<CTask<T>> routines)
		{
			var allRoutine = Get<CTask<T[]>>(true);
			allRoutine.Trace(1);
			var isCompleted = true;
			var currentId = allRoutine.id;
			foreach (var routine in routines)
			{
				routine.SetParent(allRoutine);
				routine.Start();
				if (allRoutine.id != currentId)
				{
					Assert.IsTrue(allRoutine.IsDead);
					return allRoutine;
				}
				if (!routine.IsCompleted)
				{
					routine.OnCompleted(allRoutine.StepAll<T>);
					isCompleted = false;
				}
			}
			if (isCompleted)
			{
				allRoutine.StepAll<T>();
			}
			return allRoutine;
		}

		/// <summary>
		/// Routine that yields until all routines in a collection complete. Returns array of results.
		/// </summary>
		public static CTask<T[]> WaitForAll<T>(params CTask<T>[] routines)
		{
			var allRoutine = WaitForAll((IEnumerable<CTask<T>>)routines);
			allRoutine.Trace(1);
			return allRoutine;
		}

		/// <summary>
		/// Routine that yields until the first routine in a collection completes. The others will be stopped at that
		/// time.
		/// </summary>
		public static CTask WaitForAny(IEnumerable<RoutineBase> routines)
		{
			var anyRoutine = Get<CTask>(true);
			anyRoutine.Trace(1);
			anyRoutine.stopChildrenOnStep = true;
			var isCompleted = false;
			var currentId = anyRoutine.id;
			foreach (var routine in routines)
			{
				routine.SetParent(anyRoutine);
				if (!isCompleted)
				{
					routine.Start();
					if (anyRoutine.id != currentId)
					{
						Assert.IsTrue(anyRoutine.IsDead);
						return anyRoutine;
					}
					isCompleted = routine.IsCompleted;
					if (!isCompleted)
					{
						routine.OnCompleted(anyRoutine.Step);
					}
				}
			}
			if (anyRoutine.children.Count == 0 || isCompleted)
			{
				anyRoutine.Step();
			}
			return anyRoutine;
		}

		/// <summary>
		/// Routine that yields until the first routine in a collection completes. The others will be stopped at that
		/// time.
		/// </summary>
		public static CTask WaitForAny(params RoutineBase[] routines)
		{
			var anyRoutines = WaitForAny((IEnumerable<RoutineBase>)routines);
			anyRoutines.Trace(1);
			return anyRoutines;
		}

		/// <summary>
		/// Routine that yields until the first routine in a collection completes. The others will be stopped at that
		/// time. Returns result from completed routine.
		/// </summary>
		public static CTask<T> WaitForAny<T>(IEnumerable<CTask<T>> routines)
		{
			var anyRoutine = Get<CTask<T>>(true);
			anyRoutine.Trace(1);
			anyRoutine.stopChildrenOnStep = true;
			var isCompleted = false;
			var currentId = anyRoutine.id;
			foreach (var routine in routines)
			{
				routine.SetParent(anyRoutine);
				if (!isCompleted)
				{
					routine.Start();
					if (anyRoutine.id != currentId)
					{
						Assert.IsTrue(anyRoutine.IsDead);
						return anyRoutine;
					}
					isCompleted = routine.IsCompleted;
					if (!isCompleted)
					{
						routine.OnCompleted(anyRoutine.StepAny);
					}
				}
			}
			if (anyRoutine.children.Count == 0 || isCompleted)
			{
				anyRoutine.StepAny();
			}
			return anyRoutine;
		}

		/// <summary>
		/// Routine that yields until the first routine in a collection completes. The others will be stopped at that
		/// time. Returns result from completed routine.
		/// </summary>
		public static CTask<T> WaitForAny<T>(params CTask<T>[] routines)
		{
			var anyRoutine = WaitForAny((IEnumerable<CTask<T>>)routines);
			anyRoutine.Trace(1);
			return anyRoutine;
		}

		/// <summary> Routine that yields for a set amount of time. Uses Unity game time. </summary>
		public static async CTask WaitForSeconds(float seconds)
		{
			var endTime = Time.time + seconds;
			while (Time.time < endTime)
			{
				await WaitForNextFrame();
			}
		}

		/// <summary> Routine that yields until a condition has been met. </summary>
		public static async CTask WaitUntil(Func<bool> condition)
		{
			while (!condition())
			{
				await WaitForNextFrame();
			}
		}

		/// <summary>
		/// Routine that yields until a resumer is resumed.
		/// Useful for using resumers in WaitForAll/WaitForAny.
		/// </summary>
		public static async CTask WaitForResumer(IResumer resumer)
		{
			await resumer;
		}

		/// <summary>
		/// Routine that yields until a resumer is resumed.
		/// Useful for using resumers in WaitForAll/WaitForAny.
		/// </summary>
		public static async CTask<T> WaitForResumer<T>(IResumer<T> resumer)
		{
			return await resumer;
		}

		/// <summary> Routine that yields until an AsyncOperation has completed. </summary>
		public static async CTask WaitForAsyncOperation(AsyncOperation asyncOperation)
		{
			if (!asyncOperation.isDone)
			{
				var resumer = GetResumer<AsyncOperation>();
				asyncOperation.completed += resumer.Resume;
				await resumer;
				ReleaseResumer(resumer);
			}
		}

		/// <summary> Routine that yields until a CustomYieldInstruction has completed. </summary>
		public static async CTask WaitForCustomYieldInstruction(CustomYieldInstruction customYieldInstruction)
		{
			while (customYieldInstruction.keepWaiting)
			{
				await WaitForNextFrame();
			}
		}

		public void Start()
		{
			if (manager == null)
			{
				throw new Exception("Routine is not associated with a manager!");
			}
			if (state == State.NotStarted)
			{
				Step();
			}
		}

		protected void Stop(Exception exception)
		{
			id = 0;
			ReleaseChildren();
			stopChildrenOnStep = false;
			onFinish = null;
			if (onStop != null)
			{
				onStop(exception);
				onStop = null;
			}
		}

		protected virtual void Reset()
		{
			Stop();
			state = State.NotStarted;
			if (stateMachine != null)
			{
				stateMachinePool.Release(stateMachine);
				stateMachine = null;
			}
			parent = null;
			manager = null;
		}

		protected void OnException(Exception exception)
		{
			var root = this;
			while (root.parent != null)
			{
				root = root.parent;
			}
#if DEBUG
			if (TracingEnabled)
			{
				exception = new Exception(
					string.Format("{0}\n----Async Stack----\n{1}\n---End Async Stack---", exception.Message, StackTrace),
					exception
				);
			}
#endif
			root.Stop(exception);
		}

		private void Setup(bool yield, RoutineBase parent)
		{
			id = nextId++;
			SetParent(parent);
			state = yield ? State.Running : State.NotStarted;
		}

		private void ReleaseChildren()
		{
			foreach (var child in children)
			{
				Release(child);
			}
			children.Clear();
		}

		protected void SetParent(RoutineBase newParent)
		{
			if (parent == newParent)
			{
				return;
			}

			if (parent != null)
			{
				Assert.IsTrue(parent.children.Contains(this));
				parent.children.Remove(this);
			}

			parent = newParent;

			if (newParent != null)
			{
				manager = parent.manager;
				newParent.children.Add(this);
			}
			else
			{
				manager = null;
			}
		}
	}

	[AsyncMethodBuilder(typeof(CTask))]
	public class CTask : RoutineBase
	{
		/// <summary> Assign a manager and stop handler to a routine. </summary>
		public void SetManager(RoutineManager manager, Action<Exception> onStop)
		{
			SetParent(null);
			this.manager = manager;
			this.onStop = onStop;
		}

		/// <summary> Internal use only. Required for awaiter. </summary>
		public void GetResult() {}

		/// <summary> Internal use only. Required for task-like. </summary>
		public CTask GetAwaiter()
		{
			Start(); //Step when passed to await
			return this;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			var stateMachineRef = stateMachinePool.Get<StateMachineRef<TStateMachine>>();
			stateMachineRef.value = stateMachine;
			this.stateMachine = stateMachineRef;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetStateMachine(IAsyncStateMachine stateMachine) {}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetResult()
		{
			Assert.IsTrue(state != State.Finished);
			state = State.Finished;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetException(Exception exception)
		{
			OnException(exception);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.OnCompleted(Step);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.OnCompleted(Step);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public CTask Task { get { return this; } }

		/// <summary> Internal use only. Steps a routine only if all of it's children are finished. </summary>
		public void StepAll()
		{
			foreach (var child in children)
			{
				if (!child.IsCompleted)
				{
					return;
				}
			}

			Step();
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public static CTask Create()
		{
			var routine = Get<CTask>(false);
			routine.Trace(2);
			return routine;
		}
	}

	[AsyncMethodBuilder(typeof(CTask<>))]
	public class CTask<T> : RoutineBase
	{
		private T result = default(T);

		/// <summary> Internal use only. Required for awaiter. </summary>
		public T GetResult() { return result; }

		/// <summary> Internal use only. Required for task-like. </summary>
		public CTask<T> GetAwaiter()
		{
			Start(); //Step when passed to await
			return this;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			var stateMachineRef = stateMachinePool.Get<StateMachineRef<TStateMachine>>();
			stateMachineRef.value = stateMachine;
			this.stateMachine = stateMachineRef;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetStateMachine(IAsyncStateMachine stateMachine) {}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetResult(T result)
		{
			this.result = result;
			Assert.IsTrue(state != State.Finished);
			state = State.Finished;
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void SetException(Exception exception)
		{
			OnException(exception);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.OnCompleted(Step);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.OnCompleted(Step);
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public CTask<T> Task { get { return this; } }

		/// <summary>
		/// Internal use only. Steps a routine only if all of it's children are finished and sets the result array.
		/// </summary>
		public void StepAll<I>()
		{
			foreach (var child in children)
			{
				if (!child.IsCompleted)
				{
					return;
				}
			}

			var resultArray = new I[children.Count];
			for (var i = 0; i < children.Count; ++i)
			{
				resultArray[i] = (children[i] as CTask<I>).GetResult();
			}

			(this as CTask<I[]>).SetResult(resultArray);

			Step();
		}

		/// <summary> Internal use only. Steps a routine and sets it's result from the first completed child. </summary>
		public void StepAny()
		{
			foreach (var child in children)
			{
				if (child.IsCompleted)
				{
					SetResult((child as CTask<T>).GetResult());
					break;
				}
			}

			Step();
		}

		/// <summary> Internal use only. Required for task builder. </summary>
		public static CTask<T> Create()
		{
			var routine = Get<CTask<T>>(false);
			routine.Trace(2);
			return routine;
		}

		protected override void Reset()
		{
			result = default(T);
			base.Reset();
		}
	}

	//Extensions to allow certain types to be awaited with using Routine.WaitFor
	public static class RoutineExtensions
	{
		public static CTask GetAwaiter(this AsyncOperation asyncOperation)
		{
			return CTask.WaitForAsyncOperation(asyncOperation).GetAwaiter();
		}

		public static CTask GetAwaiter(this CustomYieldInstruction customYieldInstruction)
		{
			return CTask.WaitForCustomYieldInstruction(customYieldInstruction).GetAwaiter();
		}

		public static CTask GetAwaiter(this IResumer resumer)
		{
			var _resumer = resumer as Resumer;
			Assert.IsNotNull(_resumer);
			var resumerRoutine = CTask.Get<CTask>(true);
			resumerRoutine.Trace(1);
			_resumer.routine = resumerRoutine;
			_resumer.id = resumerRoutine.Id;
			if (_resumer.WasResumed)
			{
				resumerRoutine.SetResult();
				_resumer.Reset();
			}
			return resumerRoutine;
		}

		public static CTask<T> GetAwaiter<T>(this IResumer<T> resumer)
		{
			var _resumer = resumer as Resumer<T>;
			Assert.IsNotNull(_resumer);
			var resumerRoutine = CTask.Get<CTask<T>>(true);
			resumerRoutine.Trace(1);
			_resumer.routine = resumerRoutine;
			_resumer.id = resumerRoutine.Id;
			if (_resumer.WasResumed)
			{
				resumerRoutine.SetResult(_resumer.result);
				_resumer.Reset();
			}
			return resumerRoutine;
		}
	}
}

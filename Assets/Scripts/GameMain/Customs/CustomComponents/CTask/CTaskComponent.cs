using System;
using Fuse.Tasks;
using UnityGameFramework.Runtime;

namespace Fuse
{
    //https://github.com/tomblind/unity-async-routines
    
    public class CTaskComponent: GameFrameworkComponent
    {
        public RoutineManager Manager { get { return routineManager; } }

        private RoutineManager routineManager = new RoutineManager();

        public CTaskComponent()
        {
            CTask.TracingEnabled = false;
        }
        public virtual void Update()
        {
            routineManager.Update();
        }

        public virtual void LateUpdate()
        {
            routineManager.Flush();
        }

        public void OnDestroy()
        {
            routineManager.StopAll();
        }

        /// <summary> Manages and runs a routine. </summary>
        public CTaskHandle Run(CTask routine, Action<Exception> onStop = null)
        {
            return routineManager.Run(routine, onStop);
        }

        /// <summary> Stops all managed routines. </summary>
        public void StopAll()
        {
            routineManager.StopAll();
        }
    }
}

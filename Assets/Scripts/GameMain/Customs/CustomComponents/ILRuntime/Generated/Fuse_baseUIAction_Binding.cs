using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class Fuse_baseUIAction_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.baseUIAction);

            field = type.GetField("InitUserData", flag);
            app.RegisterCLRFieldGetter(field, get_InitUserData_0);
            app.RegisterCLRFieldSetter(field, set_InitUserData_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_InitUserData_0, AssignFromStack_InitUserData_0);
            field = type.GetField("OnInit", flag);
            app.RegisterCLRFieldGetter(field, get_OnInit_1);
            app.RegisterCLRFieldSetter(field, set_OnInit_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnInit_1, AssignFromStack_OnInit_1);
            field = type.GetField("OnOpen", flag);
            app.RegisterCLRFieldGetter(field, get_OnOpen_2);
            app.RegisterCLRFieldSetter(field, set_OnOpen_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnOpen_2, AssignFromStack_OnOpen_2);
            field = type.GetField("OnClose", flag);
            app.RegisterCLRFieldGetter(field, get_OnClose_3);
            app.RegisterCLRFieldSetter(field, set_OnClose_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnClose_3, AssignFromStack_OnClose_3);
            field = type.GetField("OnPause", flag);
            app.RegisterCLRFieldGetter(field, get_OnPause_4);
            app.RegisterCLRFieldSetter(field, set_OnPause_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnPause_4, AssignFromStack_OnPause_4);
            field = type.GetField("OnResume", flag);
            app.RegisterCLRFieldGetter(field, get_OnResume_5);
            app.RegisterCLRFieldSetter(field, set_OnResume_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnResume_5, AssignFromStack_OnResume_5);
            field = type.GetField("OnCover", flag);
            app.RegisterCLRFieldGetter(field, get_OnCover_6);
            app.RegisterCLRFieldSetter(field, set_OnCover_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnCover_6, AssignFromStack_OnCover_6);
            field = type.GetField("OnReveal", flag);
            app.RegisterCLRFieldGetter(field, get_OnReveal_7);
            app.RegisterCLRFieldSetter(field, set_OnReveal_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnReveal_7, AssignFromStack_OnReveal_7);
            field = type.GetField("OnRefocus", flag);
            app.RegisterCLRFieldGetter(field, get_OnRefocus_8);
            app.RegisterCLRFieldSetter(field, set_OnRefocus_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnRefocus_8, AssignFromStack_OnRefocus_8);
            field = type.GetField("OnUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_OnUpdate_9);
            app.RegisterCLRFieldSetter(field, set_OnUpdate_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnUpdate_9, AssignFromStack_OnUpdate_9);
            field = type.GetField("OnDepthChanged", flag);
            app.RegisterCLRFieldGetter(field, get_OnDepthChanged_10);
            app.RegisterCLRFieldSetter(field, set_OnDepthChanged_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnDepthChanged_10, AssignFromStack_OnDepthChanged_10);
            field = type.GetField("OnDestroy", flag);
            app.RegisterCLRFieldGetter(field, get_OnDestroy_11);
            app.RegisterCLRFieldSetter(field, set_OnDestroy_11);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnDestroy_11, AssignFromStack_OnDestroy_11);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_InitUserData_0(ref object o)
        {
            return ((Fuse.baseUIAction)o).InitUserData;
        }

        static StackObject* CopyToStack_InitUserData_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).InitUserData;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_InitUserData_0(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).InitUserData = (System.Object)v;
        }

        static StackObject* AssignFromStack_InitUserData_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @InitUserData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).InitUserData = @InitUserData;
            return ptr_of_this_method;
        }

        static object get_OnInit_1(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnInit;
        }

        static StackObject* CopyToStack_OnInit_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnInit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnInit_1(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnInit = (System.Action<UnityEngine.GameObject, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnInit_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Object> @OnInit = (System.Action<UnityEngine.GameObject, System.Object>)typeof(System.Action<UnityEngine.GameObject, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnInit = @OnInit;
            return ptr_of_this_method;
        }

        static object get_OnOpen_2(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnOpen;
        }

        static StackObject* CopyToStack_OnOpen_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnOpen;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnOpen_2(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnOpen = (System.Action<UnityEngine.GameObject, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnOpen_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Object> @OnOpen = (System.Action<UnityEngine.GameObject, System.Object>)typeof(System.Action<UnityEngine.GameObject, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnOpen = @OnOpen;
            return ptr_of_this_method;
        }

        static object get_OnClose_3(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnClose;
        }

        static StackObject* CopyToStack_OnClose_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnClose;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnClose_3(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnClose = (System.Action<System.Boolean, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnClose_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean, System.Object> @OnClose = (System.Action<System.Boolean, System.Object>)typeof(System.Action<System.Boolean, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnClose = @OnClose;
            return ptr_of_this_method;
        }

        static object get_OnPause_4(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnPause;
        }

        static StackObject* CopyToStack_OnPause_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnPause;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnPause_4(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnPause = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnPause_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnPause = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnPause = @OnPause;
            return ptr_of_this_method;
        }

        static object get_OnResume_5(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnResume;
        }

        static StackObject* CopyToStack_OnResume_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnResume;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnResume_5(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnResume = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnResume_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnResume = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnResume = @OnResume;
            return ptr_of_this_method;
        }

        static object get_OnCover_6(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnCover;
        }

        static StackObject* CopyToStack_OnCover_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnCover;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnCover_6(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnCover = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnCover_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnCover = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnCover = @OnCover;
            return ptr_of_this_method;
        }

        static object get_OnReveal_7(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnReveal;
        }

        static StackObject* CopyToStack_OnReveal_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnReveal;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnReveal_7(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnReveal = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnReveal_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnReveal = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnReveal = @OnReveal;
            return ptr_of_this_method;
        }

        static object get_OnRefocus_8(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnRefocus;
        }

        static StackObject* CopyToStack_OnRefocus_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnRefocus;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnRefocus_8(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnRefocus = (System.Action<System.Object>)v;
        }

        static StackObject* AssignFromStack_OnRefocus_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Object> @OnRefocus = (System.Action<System.Object>)typeof(System.Action<System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnRefocus = @OnRefocus;
            return ptr_of_this_method;
        }

        static object get_OnUpdate_9(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnUpdate;
        }

        static StackObject* CopyToStack_OnUpdate_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnUpdate;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnUpdate_9(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnUpdate = (System.Action<System.Single, System.Single>)v;
        }

        static StackObject* AssignFromStack_OnUpdate_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Single, System.Single> @OnUpdate = (System.Action<System.Single, System.Single>)typeof(System.Action<System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnUpdate = @OnUpdate;
            return ptr_of_this_method;
        }

        static object get_OnDepthChanged_10(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnDepthChanged;
        }

        static StackObject* CopyToStack_OnDepthChanged_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnDepthChanged;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnDepthChanged_10(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnDepthChanged = (System.Action<System.Int32, System.Int32>)v;
        }

        static StackObject* AssignFromStack_OnDepthChanged_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32, System.Int32> @OnDepthChanged = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnDepthChanged = @OnDepthChanged;
            return ptr_of_this_method;
        }

        static object get_OnDestroy_11(ref object o)
        {
            return ((Fuse.baseUIAction)o).OnDestroy;
        }

        static StackObject* CopyToStack_OnDestroy_11(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseUIAction)o).OnDestroy;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnDestroy_11(ref object o, object v)
        {
            ((Fuse.baseUIAction)o).OnDestroy = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnDestroy_11(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnDestroy = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseUIAction)o).OnDestroy = @OnDestroy;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Fuse.baseUIAction();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}

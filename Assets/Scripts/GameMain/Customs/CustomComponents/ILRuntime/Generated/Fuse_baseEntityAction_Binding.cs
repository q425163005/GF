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
    unsafe class Fuse_baseEntityAction_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.baseEntityAction);

            field = type.GetField("HotfixObj", flag);
            app.RegisterCLRFieldGetter(field, get_HotfixObj_0);
            app.RegisterCLRFieldSetter(field, set_HotfixObj_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_HotfixObj_0, AssignFromStack_HotfixObj_0);
            field = type.GetField("InitUserData", flag);
            app.RegisterCLRFieldGetter(field, get_InitUserData_1);
            app.RegisterCLRFieldSetter(field, set_InitUserData_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_InitUserData_1, AssignFromStack_InitUserData_1);
            field = type.GetField("OnInit", flag);
            app.RegisterCLRFieldGetter(field, get_OnInit_2);
            app.RegisterCLRFieldSetter(field, set_OnInit_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnInit_2, AssignFromStack_OnInit_2);
            field = type.GetField("OnRecycle", flag);
            app.RegisterCLRFieldGetter(field, get_OnRecycle_3);
            app.RegisterCLRFieldSetter(field, set_OnRecycle_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnRecycle_3, AssignFromStack_OnRecycle_3);
            field = type.GetField("OnShow", flag);
            app.RegisterCLRFieldGetter(field, get_OnShow_4);
            app.RegisterCLRFieldSetter(field, set_OnShow_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnShow_4, AssignFromStack_OnShow_4);
            field = type.GetField("OnHide", flag);
            app.RegisterCLRFieldGetter(field, get_OnHide_5);
            app.RegisterCLRFieldSetter(field, set_OnHide_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnHide_5, AssignFromStack_OnHide_5);
            field = type.GetField("OnAttached", flag);
            app.RegisterCLRFieldGetter(field, get_OnAttached_6);
            app.RegisterCLRFieldSetter(field, set_OnAttached_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnAttached_6, AssignFromStack_OnAttached_6);
            field = type.GetField("OnDetached", flag);
            app.RegisterCLRFieldGetter(field, get_OnDetached_7);
            app.RegisterCLRFieldSetter(field, set_OnDetached_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnDetached_7, AssignFromStack_OnDetached_7);
            field = type.GetField("OnAttachTo", flag);
            app.RegisterCLRFieldGetter(field, get_OnAttachTo_8);
            app.RegisterCLRFieldSetter(field, set_OnAttachTo_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnAttachTo_8, AssignFromStack_OnAttachTo_8);
            field = type.GetField("OnDetachFrom", flag);
            app.RegisterCLRFieldGetter(field, get_OnDetachFrom_9);
            app.RegisterCLRFieldSetter(field, set_OnDetachFrom_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnDetachFrom_9, AssignFromStack_OnDetachFrom_9);
            field = type.GetField("OnUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_OnUpdate_10);
            app.RegisterCLRFieldSetter(field, set_OnUpdate_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnUpdate_10, AssignFromStack_OnUpdate_10);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_HotfixObj_0(ref object o)
        {
            return ((Fuse.baseEntityAction)o).HotfixObj;
        }

        static StackObject* CopyToStack_HotfixObj_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).HotfixObj;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_HotfixObj_0(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).HotfixObj = (System.Object)v;
        }

        static StackObject* AssignFromStack_HotfixObj_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @HotfixObj = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).HotfixObj = @HotfixObj;
            return ptr_of_this_method;
        }

        static object get_InitUserData_1(ref object o)
        {
            return ((Fuse.baseEntityAction)o).InitUserData;
        }

        static StackObject* CopyToStack_InitUserData_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).InitUserData;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_InitUserData_1(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).InitUserData = (System.Object)v;
        }

        static StackObject* AssignFromStack_InitUserData_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @InitUserData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).InitUserData = @InitUserData;
            return ptr_of_this_method;
        }

        static object get_OnInit_2(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnInit;
        }

        static StackObject* CopyToStack_OnInit_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnInit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnInit_2(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnInit = (System.Action<UnityEngine.GameObject, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnInit_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Object> @OnInit = (System.Action<UnityEngine.GameObject, System.Object>)typeof(System.Action<UnityEngine.GameObject, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnInit = @OnInit;
            return ptr_of_this_method;
        }

        static object get_OnRecycle_3(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnRecycle;
        }

        static StackObject* CopyToStack_OnRecycle_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnRecycle;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnRecycle_3(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnRecycle = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnRecycle_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnRecycle = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnRecycle = @OnRecycle;
            return ptr_of_this_method;
        }

        static object get_OnShow_4(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnShow;
        }

        static StackObject* CopyToStack_OnShow_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnShow;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnShow_4(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnShow = (System.Action<UnityEngine.GameObject, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnShow_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Object> @OnShow = (System.Action<UnityEngine.GameObject, System.Object>)typeof(System.Action<UnityEngine.GameObject, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnShow = @OnShow;
            return ptr_of_this_method;
        }

        static object get_OnHide_5(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnHide;
        }

        static StackObject* CopyToStack_OnHide_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnHide;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnHide_5(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnHide = (System.Action<System.Boolean, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnHide_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean, System.Object> @OnHide = (System.Action<System.Boolean, System.Object>)typeof(System.Action<System.Boolean, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnHide = @OnHide;
            return ptr_of_this_method;
        }

        static object get_OnAttached_6(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnAttached;
        }

        static StackObject* CopyToStack_OnAttached_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnAttached;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnAttached_6(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnAttached = (System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnAttached_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object> @OnAttached = (System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>)typeof(System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnAttached = @OnAttached;
            return ptr_of_this_method;
        }

        static object get_OnDetached_7(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnDetached;
        }

        static StackObject* CopyToStack_OnDetached_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnDetached;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnDetached_7(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnDetached = (System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnDetached_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object> @OnDetached = (System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>)typeof(System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnDetached = @OnDetached;
            return ptr_of_this_method;
        }

        static object get_OnAttachTo_8(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnAttachTo;
        }

        static StackObject* CopyToStack_OnAttachTo_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnAttachTo;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnAttachTo_8(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnAttachTo = (System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnAttachTo_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object> @OnAttachTo = (System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>)typeof(System.Action<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnAttachTo = @OnAttachTo;
            return ptr_of_this_method;
        }

        static object get_OnDetachFrom_9(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnDetachFrom;
        }

        static StackObject* CopyToStack_OnDetachFrom_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnDetachFrom;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnDetachFrom_9(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnDetachFrom = (System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>)v;
        }

        static StackObject* AssignFromStack_OnDetachFrom_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object> @OnDetachFrom = (System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>)typeof(System.Action<UnityGameFramework.Runtime.EntityLogic, System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnDetachFrom = @OnDetachFrom;
            return ptr_of_this_method;
        }

        static object get_OnUpdate_10(ref object o)
        {
            return ((Fuse.baseEntityAction)o).OnUpdate;
        }

        static StackObject* CopyToStack_OnUpdate_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.baseEntityAction)o).OnUpdate;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnUpdate_10(ref object o, object v)
        {
            ((Fuse.baseEntityAction)o).OnUpdate = (System.Action<System.Single, System.Single>)v;
        }

        static StackObject* AssignFromStack_OnUpdate_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Single, System.Single> @OnUpdate = (System.Action<System.Single, System.Single>)typeof(System.Action<System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.baseEntityAction)o).OnUpdate = @OnUpdate;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Fuse.baseEntityAction();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}

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
    unsafe class UnityGameFramework_Runtime_UIComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(UnityGameFramework.Runtime.UIComponent);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("GetUIForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetUIForm_0);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.Int32), typeof(System.Boolean), typeof(System.Object)};
            method = type.GetMethod("OpenUIForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OpenUIForm_1);
            args = new Type[]{typeof(UnityGameFramework.Runtime.UIForm)};
            method = type.GetMethod("CloseUIForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseUIForm_2);
            args = new Type[]{typeof(UnityGameFramework.Runtime.UIForm), typeof(System.Object)};
            method = type.GetMethod("CloseUIForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseUIForm_3);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("CloseUIForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseUIForm_4);
            args = new Type[]{typeof(UnityGameFramework.Runtime.UIForm), typeof(System.Boolean)};
            method = type.GetMethod("SetUIFormInstanceLocked", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUIFormInstanceLocked_5);
            args = new Type[]{};
            method = type.GetMethod("CloseAllLoadingUIForms", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseAllLoadingUIForms_6);
            args = new Type[]{};
            method = type.GetMethod("GetAllLoadedUIForms", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAllLoadedUIForms_7);
            args = new Type[]{};
            method = type.GetMethod("GetAllLoadingUIFormSerialIds", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAllLoadingUIFormSerialIds_8);
            args = new Type[]{typeof(System.String), typeof(System.Int32)};
            method = type.GetMethod("AddUIGroup", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddUIGroup_9);


        }


        static StackObject* GetUIForm_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @uiFormAssetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetUIForm(@uiFormAssetName);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* OpenUIForm_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @userData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @pauseCoveredUIForm = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @priority = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @uiGroupName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @uiFormAssetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.OpenUIForm(@uiFormAssetName, @uiGroupName, @priority, @pauseCoveredUIForm, @userData);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* CloseUIForm_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.UIForm @uiForm = (UnityGameFramework.Runtime.UIForm)typeof(UnityGameFramework.Runtime.UIForm).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseUIForm(@uiForm);

            return __ret;
        }

        static StackObject* CloseUIForm_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @userData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityGameFramework.Runtime.UIForm @uiForm = (UnityGameFramework.Runtime.UIForm)typeof(UnityGameFramework.Runtime.UIForm).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseUIForm(@uiForm, @userData);

            return __ret;
        }

        static StackObject* CloseUIForm_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @serialId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseUIForm(@serialId);

            return __ret;
        }

        static StackObject* SetUIFormInstanceLocked_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @locked = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityGameFramework.Runtime.UIForm @uiForm = (UnityGameFramework.Runtime.UIForm)typeof(UnityGameFramework.Runtime.UIForm).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetUIFormInstanceLocked(@uiForm, @locked);

            return __ret;
        }

        static StackObject* CloseAllLoadingUIForms_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseAllLoadingUIForms();

            return __ret;
        }

        static StackObject* GetAllLoadedUIForms_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllLoadedUIForms();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAllLoadingUIFormSerialIds_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllLoadingUIFormSerialIds();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddUIGroup_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @depth = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @uiGroupName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.UIComponent instance_of_this_method = (UnityGameFramework.Runtime.UIComponent)typeof(UnityGameFramework.Runtime.UIComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AddUIGroup(@uiGroupName, @depth);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}

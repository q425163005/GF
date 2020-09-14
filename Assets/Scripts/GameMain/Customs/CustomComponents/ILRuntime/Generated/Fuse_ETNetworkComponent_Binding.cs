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
    unsafe class Fuse_ETNetworkComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.ETNetworkComponent);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("CreateSession", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CreateSession_0);

            field = type.GetField("ServerIP", flag);
            app.RegisterCLRFieldGetter(field, get_ServerIP_0);
            app.RegisterCLRFieldSetter(field, set_ServerIP_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ServerIP_0, AssignFromStack_ServerIP_0);


        }


        static StackObject* CreateSession_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @address = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Fuse.ETNetworkComponent instance_of_this_method = (Fuse.ETNetworkComponent)typeof(Fuse.ETNetworkComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CreateSession(@address);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_ServerIP_0(ref object o)
        {
            return ((Fuse.ETNetworkComponent)o).ServerIP;
        }

        static StackObject* CopyToStack_ServerIP_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.ETNetworkComponent)o).ServerIP;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ServerIP_0(ref object o, object v)
        {
            ((Fuse.ETNetworkComponent)o).ServerIP = (System.String)v;
        }

        static StackObject* AssignFromStack_ServerIP_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @ServerIP = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.ETNetworkComponent)o).ServerIP = @ServerIP;
            return ptr_of_this_method;
        }



    }
}

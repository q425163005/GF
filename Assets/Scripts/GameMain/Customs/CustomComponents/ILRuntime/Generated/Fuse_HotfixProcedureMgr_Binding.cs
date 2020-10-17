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
    unsafe class Fuse_HotfixProcedureMgr_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.HotfixProcedureMgr);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_nowHotfixProcedure", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_nowHotfixProcedure_0);

            field = type.GetField("allHotfixProcedure", flag);
            app.RegisterCLRFieldGetter(field, get_allHotfixProcedure_0);
            app.RegisterCLRFieldSetter(field, set_allHotfixProcedure_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_allHotfixProcedure_0, AssignFromStack_allHotfixProcedure_0);


        }


        static StackObject* set_nowHotfixProcedure_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Fuse.HotfixProcedureMgr instance_of_this_method = (Fuse.HotfixProcedureMgr)typeof(Fuse.HotfixProcedureMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.nowHotfixProcedure = value;

            return __ret;
        }


        static object get_allHotfixProcedure_0(ref object o)
        {
            return ((Fuse.HotfixProcedureMgr)o).allHotfixProcedure;
        }

        static StackObject* CopyToStack_allHotfixProcedure_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.HotfixProcedureMgr)o).allHotfixProcedure;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_allHotfixProcedure_0(ref object o, object v)
        {
            ((Fuse.HotfixProcedureMgr)o).allHotfixProcedure = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_allHotfixProcedure_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @allHotfixProcedure = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.HotfixProcedureMgr)o).allHotfixProcedure = @allHotfixProcedure;
            return ptr_of_this_method;
        }



    }
}

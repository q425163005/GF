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
    unsafe class Fuse_HotfixEntityLogic_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.HotfixEntityLogic);

            field = type.GetField("BaseEntityAction", flag);
            app.RegisterCLRFieldGetter(field, get_BaseEntityAction_0);
            app.RegisterCLRFieldSetter(field, set_BaseEntityAction_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_BaseEntityAction_0, AssignFromStack_BaseEntityAction_0);


        }



        static object get_BaseEntityAction_0(ref object o)
        {
            return ((Fuse.HotfixEntityLogic)o).BaseEntityAction;
        }

        static StackObject* CopyToStack_BaseEntityAction_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.HotfixEntityLogic)o).BaseEntityAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_BaseEntityAction_0(ref object o, object v)
        {
            ((Fuse.HotfixEntityLogic)o).BaseEntityAction = (Fuse.baseEntityAction)v;
        }

        static StackObject* AssignFromStack_BaseEntityAction_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.baseEntityAction @BaseEntityAction = (Fuse.baseEntityAction)typeof(Fuse.baseEntityAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.HotfixEntityLogic)o).BaseEntityAction = @BaseEntityAction;
            return ptr_of_this_method;
        }



    }
}

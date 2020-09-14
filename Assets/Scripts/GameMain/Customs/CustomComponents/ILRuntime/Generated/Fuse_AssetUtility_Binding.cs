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
    unsafe class Fuse_AssetUtility_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.AssetUtility);

            field = type.GetField("UpdateFormAsset", flag);
            app.RegisterCLRFieldGetter(field, get_UpdateFormAsset_0);
            app.RegisterCLRFieldSetter(field, set_UpdateFormAsset_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_UpdateFormAsset_0, AssignFromStack_UpdateFormAsset_0);


        }



        static object get_UpdateFormAsset_0(ref object o)
        {
            return Fuse.AssetUtility.UpdateFormAsset;
        }

        static StackObject* CopyToStack_UpdateFormAsset_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Fuse.AssetUtility.UpdateFormAsset;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_UpdateFormAsset_0(ref object o, object v)
        {
            Fuse.AssetUtility.UpdateFormAsset = (System.String)v;
        }

        static StackObject* AssignFromStack_UpdateFormAsset_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @UpdateFormAsset = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Fuse.AssetUtility.UpdateFormAsset = @UpdateFormAsset;
            return ptr_of_this_method;
        }



    }
}

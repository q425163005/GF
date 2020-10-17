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
    unsafe class Fuse_CompCollector_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.CompCollector);

            field = type.GetField("CompCollectorInfos", flag);
            app.RegisterCLRFieldGetter(field, get_CompCollectorInfos_0);
            app.RegisterCLRFieldSetter(field, set_CompCollectorInfos_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_CompCollectorInfos_0, AssignFromStack_CompCollectorInfos_0);


        }



        static object get_CompCollectorInfos_0(ref object o)
        {
            return ((Fuse.CompCollector)o).CompCollectorInfos;
        }

        static StackObject* CopyToStack_CompCollectorInfos_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.CompCollector)o).CompCollectorInfos;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_CompCollectorInfos_0(ref object o, object v)
        {
            ((Fuse.CompCollector)o).CompCollectorInfos = (System.Collections.Generic.List<Fuse.CompCollector.CompCollectorInfo>)v;
        }

        static StackObject* AssignFromStack_CompCollectorInfos_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<Fuse.CompCollector.CompCollectorInfo> @CompCollectorInfos = (System.Collections.Generic.List<Fuse.CompCollector.CompCollectorInfo>)typeof(System.Collections.Generic.List<Fuse.CompCollector.CompCollectorInfo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.CompCollector)o).CompCollectorInfos = @CompCollectorInfos;
            return ptr_of_this_method;
        }



    }
}

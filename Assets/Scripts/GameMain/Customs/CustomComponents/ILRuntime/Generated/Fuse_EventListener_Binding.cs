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
    unsafe class Fuse_EventListener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Fuse.EventListener);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_0);

            field = type.GetField("onClick", flag);
            app.RegisterCLRFieldGetter(field, get_onClick_0);
            app.RegisterCLRFieldSetter(field, set_onClick_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onClick_0, AssignFromStack_onClick_0);
            field = type.GetField("onDown", flag);
            app.RegisterCLRFieldGetter(field, get_onDown_1);
            app.RegisterCLRFieldSetter(field, set_onDown_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDown_1, AssignFromStack_onDown_1);
            field = type.GetField("onUp", flag);
            app.RegisterCLRFieldGetter(field, get_onUp_2);
            app.RegisterCLRFieldSetter(field, set_onUp_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onUp_2, AssignFromStack_onUp_2);
            field = type.GetField("onEnter", flag);
            app.RegisterCLRFieldGetter(field, get_onEnter_3);
            app.RegisterCLRFieldSetter(field, set_onEnter_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onEnter_3, AssignFromStack_onEnter_3);
            field = type.GetField("onExit", flag);
            app.RegisterCLRFieldGetter(field, get_onExit_4);
            app.RegisterCLRFieldSetter(field, set_onExit_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_onExit_4, AssignFromStack_onExit_4);


        }


        static StackObject* Get_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Fuse.EventListener.Get(@go);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_onClick_0(ref object o)
        {
            return ((Fuse.EventListener)o).onClick;
        }

        static StackObject* CopyToStack_onClick_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onClick_0(ref object o, object v)
        {
            ((Fuse.EventListener)o).onClick = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onClick_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onClick = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onClick = @onClick;
            return ptr_of_this_method;
        }

        static object get_onDown_1(ref object o)
        {
            return ((Fuse.EventListener)o).onDown;
        }

        static StackObject* CopyToStack_onDown_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDown;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDown_1(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDown = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDown_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onDown = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDown = @onDown;
            return ptr_of_this_method;
        }

        static object get_onUp_2(ref object o)
        {
            return ((Fuse.EventListener)o).onUp;
        }

        static StackObject* CopyToStack_onUp_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onUp;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onUp_2(ref object o, object v)
        {
            ((Fuse.EventListener)o).onUp = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onUp_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onUp = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onUp = @onUp;
            return ptr_of_this_method;
        }

        static object get_onEnter_3(ref object o)
        {
            return ((Fuse.EventListener)o).onEnter;
        }

        static StackObject* CopyToStack_onEnter_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onEnter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onEnter_3(ref object o, object v)
        {
            ((Fuse.EventListener)o).onEnter = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onEnter_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onEnter = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onEnter = @onEnter;
            return ptr_of_this_method;
        }

        static object get_onExit_4(ref object o)
        {
            return ((Fuse.EventListener)o).onExit;
        }

        static StackObject* CopyToStack_onExit_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onExit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onExit_4(ref object o, object v)
        {
            ((Fuse.EventListener)o).onExit = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onExit_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onExit = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onExit = @onExit;
            return ptr_of_this_method;
        }



    }
}

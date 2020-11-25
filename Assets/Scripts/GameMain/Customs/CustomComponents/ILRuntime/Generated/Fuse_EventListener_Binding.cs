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
            args = new Type[]{typeof(UnityEngine.Component)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_1);

            field = type.GetField("onClick", flag);
            app.RegisterCLRFieldGetter(field, get_onClick_0);
            app.RegisterCLRFieldSetter(field, set_onClick_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onClick_0, AssignFromStack_onClick_0);
            field = type.GetField("onDoubleClick", flag);
            app.RegisterCLRFieldGetter(field, get_onDoubleClick_1);
            app.RegisterCLRFieldSetter(field, set_onDoubleClick_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDoubleClick_1, AssignFromStack_onDoubleClick_1);
            field = type.GetField("onPress", flag);
            app.RegisterCLRFieldGetter(field, get_onPress_2);
            app.RegisterCLRFieldSetter(field, set_onPress_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPress_2, AssignFromStack_onPress_2);
            field = type.GetField("onUp", flag);
            app.RegisterCLRFieldGetter(field, get_onUp_3);
            app.RegisterCLRFieldSetter(field, set_onUp_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onUp_3, AssignFromStack_onUp_3);
            field = type.GetField("onDown", flag);
            app.RegisterCLRFieldGetter(field, get_onDown_4);
            app.RegisterCLRFieldSetter(field, set_onDown_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDown_4, AssignFromStack_onDown_4);
            field = type.GetField("onEnter", flag);
            app.RegisterCLRFieldGetter(field, get_onEnter_5);
            app.RegisterCLRFieldSetter(field, set_onEnter_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_onEnter_5, AssignFromStack_onEnter_5);
            field = type.GetField("onExit", flag);
            app.RegisterCLRFieldGetter(field, get_onExit_6);
            app.RegisterCLRFieldSetter(field, set_onExit_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_onExit_6, AssignFromStack_onExit_6);
            field = type.GetField("onSelect", flag);
            app.RegisterCLRFieldGetter(field, get_onSelect_7);
            app.RegisterCLRFieldSetter(field, set_onSelect_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_onSelect_7, AssignFromStack_onSelect_7);
            field = type.GetField("onUpdateSelect", flag);
            app.RegisterCLRFieldGetter(field, get_onUpdateSelect_8);
            app.RegisterCLRFieldSetter(field, set_onUpdateSelect_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_onUpdateSelect_8, AssignFromStack_onUpdateSelect_8);
            field = type.GetField("onDeselect", flag);
            app.RegisterCLRFieldGetter(field, get_onDeselect_9);
            app.RegisterCLRFieldSetter(field, set_onDeselect_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDeselect_9, AssignFromStack_onDeselect_9);
            field = type.GetField("onBeginDrag", flag);
            app.RegisterCLRFieldGetter(field, get_onBeginDrag_10);
            app.RegisterCLRFieldSetter(field, set_onBeginDrag_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_onBeginDrag_10, AssignFromStack_onBeginDrag_10);
            field = type.GetField("onDrag", flag);
            app.RegisterCLRFieldGetter(field, get_onDrag_11);
            app.RegisterCLRFieldSetter(field, set_onDrag_11);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDrag_11, AssignFromStack_onDrag_11);
            field = type.GetField("onEndDrag", flag);
            app.RegisterCLRFieldGetter(field, get_onEndDrag_12);
            app.RegisterCLRFieldSetter(field, set_onEndDrag_12);
            app.RegisterCLRFieldBinding(field, CopyToStack_onEndDrag_12, AssignFromStack_onEndDrag_12);
            field = type.GetField("onDrop", flag);
            app.RegisterCLRFieldGetter(field, get_onDrop_13);
            app.RegisterCLRFieldSetter(field, set_onDrop_13);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDrop_13, AssignFromStack_onDrop_13);
            field = type.GetField("onScroll", flag);
            app.RegisterCLRFieldGetter(field, get_onScroll_14);
            app.RegisterCLRFieldSetter(field, set_onScroll_14);
            app.RegisterCLRFieldBinding(field, CopyToStack_onScroll_14, AssignFromStack_onScroll_14);
            field = type.GetField("onMove", flag);
            app.RegisterCLRFieldGetter(field, get_onMove_15);
            app.RegisterCLRFieldSetter(field, set_onMove_15);
            app.RegisterCLRFieldBinding(field, CopyToStack_onMove_15, AssignFromStack_onMove_15);


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

        static StackObject* Get_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Component @go = (UnityEngine.Component)typeof(UnityEngine.Component).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
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

        static object get_onDoubleClick_1(ref object o)
        {
            return ((Fuse.EventListener)o).onDoubleClick;
        }

        static StackObject* CopyToStack_onDoubleClick_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDoubleClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDoubleClick_1(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDoubleClick = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDoubleClick_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onDoubleClick = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDoubleClick = @onDoubleClick;
            return ptr_of_this_method;
        }

        static object get_onPress_2(ref object o)
        {
            return ((Fuse.EventListener)o).onPress;
        }

        static StackObject* CopyToStack_onPress_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onPress;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPress_2(ref object o, object v)
        {
            ((Fuse.EventListener)o).onPress = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onPress_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onPress = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onPress = @onPress;
            return ptr_of_this_method;
        }

        static object get_onUp_3(ref object o)
        {
            return ((Fuse.EventListener)o).onUp;
        }

        static StackObject* CopyToStack_onUp_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onUp;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onUp_3(ref object o, object v)
        {
            ((Fuse.EventListener)o).onUp = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onUp_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onUp = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onUp = @onUp;
            return ptr_of_this_method;
        }

        static object get_onDown_4(ref object o)
        {
            return ((Fuse.EventListener)o).onDown;
        }

        static StackObject* CopyToStack_onDown_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDown;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDown_4(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDown = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDown_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onDown = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDown = @onDown;
            return ptr_of_this_method;
        }

        static object get_onEnter_5(ref object o)
        {
            return ((Fuse.EventListener)o).onEnter;
        }

        static StackObject* CopyToStack_onEnter_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onEnter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onEnter_5(ref object o, object v)
        {
            ((Fuse.EventListener)o).onEnter = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onEnter_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onEnter = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onEnter = @onEnter;
            return ptr_of_this_method;
        }

        static object get_onExit_6(ref object o)
        {
            return ((Fuse.EventListener)o).onExit;
        }

        static StackObject* CopyToStack_onExit_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onExit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onExit_6(ref object o, object v)
        {
            ((Fuse.EventListener)o).onExit = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onExit_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onExit = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onExit = @onExit;
            return ptr_of_this_method;
        }

        static object get_onSelect_7(ref object o)
        {
            return ((Fuse.EventListener)o).onSelect;
        }

        static StackObject* CopyToStack_onSelect_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onSelect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onSelect_7(ref object o, object v)
        {
            ((Fuse.EventListener)o).onSelect = (Fuse.EventListener.BaseDataDelegate)v;
        }

        static StackObject* AssignFromStack_onSelect_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.BaseDataDelegate @onSelect = (Fuse.EventListener.BaseDataDelegate)typeof(Fuse.EventListener.BaseDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onSelect = @onSelect;
            return ptr_of_this_method;
        }

        static object get_onUpdateSelect_8(ref object o)
        {
            return ((Fuse.EventListener)o).onUpdateSelect;
        }

        static StackObject* CopyToStack_onUpdateSelect_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onUpdateSelect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onUpdateSelect_8(ref object o, object v)
        {
            ((Fuse.EventListener)o).onUpdateSelect = (Fuse.EventListener.BaseDataDelegate)v;
        }

        static StackObject* AssignFromStack_onUpdateSelect_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.BaseDataDelegate @onUpdateSelect = (Fuse.EventListener.BaseDataDelegate)typeof(Fuse.EventListener.BaseDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onUpdateSelect = @onUpdateSelect;
            return ptr_of_this_method;
        }

        static object get_onDeselect_9(ref object o)
        {
            return ((Fuse.EventListener)o).onDeselect;
        }

        static StackObject* CopyToStack_onDeselect_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDeselect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDeselect_9(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDeselect = (Fuse.EventListener.BaseDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDeselect_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.BaseDataDelegate @onDeselect = (Fuse.EventListener.BaseDataDelegate)typeof(Fuse.EventListener.BaseDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDeselect = @onDeselect;
            return ptr_of_this_method;
        }

        static object get_onBeginDrag_10(ref object o)
        {
            return ((Fuse.EventListener)o).onBeginDrag;
        }

        static StackObject* CopyToStack_onBeginDrag_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onBeginDrag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onBeginDrag_10(ref object o, object v)
        {
            ((Fuse.EventListener)o).onBeginDrag = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onBeginDrag_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onBeginDrag = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onBeginDrag = @onBeginDrag;
            return ptr_of_this_method;
        }

        static object get_onDrag_11(ref object o)
        {
            return ((Fuse.EventListener)o).onDrag;
        }

        static StackObject* CopyToStack_onDrag_11(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDrag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDrag_11(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDrag = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDrag_11(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onDrag = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDrag = @onDrag;
            return ptr_of_this_method;
        }

        static object get_onEndDrag_12(ref object o)
        {
            return ((Fuse.EventListener)o).onEndDrag;
        }

        static StackObject* CopyToStack_onEndDrag_12(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onEndDrag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onEndDrag_12(ref object o, object v)
        {
            ((Fuse.EventListener)o).onEndDrag = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onEndDrag_12(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onEndDrag = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onEndDrag = @onEndDrag;
            return ptr_of_this_method;
        }

        static object get_onDrop_13(ref object o)
        {
            return ((Fuse.EventListener)o).onDrop;
        }

        static StackObject* CopyToStack_onDrop_13(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onDrop;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDrop_13(ref object o, object v)
        {
            ((Fuse.EventListener)o).onDrop = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onDrop_13(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onDrop = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onDrop = @onDrop;
            return ptr_of_this_method;
        }

        static object get_onScroll_14(ref object o)
        {
            return ((Fuse.EventListener)o).onScroll;
        }

        static StackObject* CopyToStack_onScroll_14(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onScroll;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onScroll_14(ref object o, object v)
        {
            ((Fuse.EventListener)o).onScroll = (Fuse.EventListener.PointerDataDelegate)v;
        }

        static StackObject* AssignFromStack_onScroll_14(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.PointerDataDelegate @onScroll = (Fuse.EventListener.PointerDataDelegate)typeof(Fuse.EventListener.PointerDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onScroll = @onScroll;
            return ptr_of_this_method;
        }

        static object get_onMove_15(ref object o)
        {
            return ((Fuse.EventListener)o).onMove;
        }

        static StackObject* CopyToStack_onMove_15(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Fuse.EventListener)o).onMove;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onMove_15(ref object o, object v)
        {
            ((Fuse.EventListener)o).onMove = (Fuse.EventListener.AxisDataDelegate)v;
        }

        static StackObject* AssignFromStack_onMove_15(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Fuse.EventListener.AxisDataDelegate @onMove = (Fuse.EventListener.AxisDataDelegate)typeof(Fuse.EventListener.AxisDataDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Fuse.EventListener)o).onMove = @onMove;
            return ptr_of_this_method;
        }



    }
}

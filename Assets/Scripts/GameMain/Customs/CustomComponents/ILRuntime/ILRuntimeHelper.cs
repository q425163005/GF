using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using Google.Protobuf;
using Fuse;
using UnityEngine;
using UnityEngine.Events;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Fuse
{
    public static class ILRuntimeHelper
    {
        public static void InitILRuntime(AppDomain appdomain)
        {
            //TODO:注册重定向方法
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());

            //TODO:适配委托
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<Vector3>();
            appdomain.DelegateManager.RegisterMethodDelegate<Vector2>();
            appdomain.DelegateManager.RegisterMethodDelegate<Vector2Int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2Int, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2, System.Single>();

            appdomain.DelegateManager
                     .RegisterMethodDelegate<System.String, System.Object, System.Single, System.Object>();
            appdomain.DelegateManager
                     .RegisterMethodDelegate<System.String, GameFramework.Resource.LoadResourceStatus, System.String,
                         System.Object>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
          



            //GF用
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<object, ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();


            //ET用
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
            appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
            appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
            appdomain.DelegateManager.RegisterMethodDelegate<Session, byte, ushort, MemoryStream>();
            appdomain.DelegateManager.RegisterMethodDelegate<Session>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<Session, ushort, MemoryStream>();

            //PB用
            appdomain.DelegateManager.RegisterFunctionDelegate<IMessageAdaptor.Adaptor>();
            appdomain.DelegateManager.RegisterMethodDelegate<IMessageAdaptor.Adaptor>();

            //HotFixUI用
            appdomain.DelegateManager.RegisterMethodDelegate<Fuse.HotfixUGuiForm, System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2, System.Boolean>();


            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                {
                    ((Action<UnityEngine.Vector2>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.Vector2>>((act) =>
            {
                return new System.Predicate<UnityEngine.Vector2>((obj) =>
                {
                    return ((Func<UnityEngine.Vector2, System.Boolean>)act)(obj);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.Vector2Int>>((act) =>
            {
                return new System.Predicate<UnityEngine.Vector2Int>((obj) =>
                {
                    return ((Func<UnityEngine.Vector2Int, System.Boolean>)act)(obj);
                });
            });



            //HotFixEntity用
            appdomain.DelegateManager.RegisterMethodDelegate<Fuse.HotfixEntityLogic, System.Object>();
            appdomain.DelegateManager
                     .RegisterMethodDelegate<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform,
                         System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityGameFramework.Runtime.EntityLogic, System.Object>();


            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
                });
            });


            #region EventListener

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
            
            appdomain.DelegateManager.RegisterDelegateConvertor<global::Fuse.EventListener.PointerDataDelegate>((act) =>
            {
                return new global::Fuse.EventListener.PointerDataDelegate((eventData) =>
                {
                    ((Action<UnityEngine.EventSystems.PointerEventData>)act)(eventData);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::Fuse.EventListener.BaseDataDelegate>((act) =>
            {
                return new global::Fuse.EventListener.BaseDataDelegate((eventData) =>
                {
                    ((Action<UnityEngine.EventSystems.BaseEventData>)act)(eventData);
                });
            });


            #endregion

            #region DOTween

            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() => { ((Action)act)(); });
            });


            #endregion

            //TODO:注册委托
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() =>{((Action)action)();});
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>((a) =>{((Action<float>) action)(a);});
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<EventHandler<GameFramework.Event.GameEventArgs>>((act) =>
            {
                return new EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                {
                    ((Action<object,GameFramework.Event.GameEventArgs>) act)(sender, e);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<EventHandler<ILTypeInstance>>((act) =>
            {
                return new EventHandler<ILTypeInstance>((sender, e) =>
                {
                    ((Action<object,ILTypeInstance>) act)(sender,e);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetSuccessCallback>((act) =>
            {
                return new GameFramework.Resource.LoadAssetSuccessCallback((assetName, asset, duration, userData) =>
                {
                    ((Action<System.String, System.Object, System.Single, System.Object>)act)(assetName, asset, duration,userData);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetFailureCallback>((act) =>
            {
                return new GameFramework.Resource.LoadAssetFailureCallback((assetName, status, errorMessage, userData) =>
                {
                    ((Action<System.String, GameFramework.Resource.LoadResourceStatus, System.String,System.Object>) act)
                        (assetName, status, errorMessage, userData);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
            {
                return new System.AsyncCallback((ar) => { ((Action<System.IAsyncResult>) act)(ar); });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ThreadStart>((act) =>
            {
                return new System.Threading.ThreadStart(() =>{((Action)act)();});
            });

            //注册CLR绑定代码
            CLRBindings.Initialize(appdomain);

            //TODO:注册跨域继承适配器
            appdomain.RegisterCrossBindingAdaptor(new IMessageAdaptor());
            appdomain.RegisterCrossBindingAdaptor(new IDisposableAdaptor());
            appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdaptor());

            //TODO:注册值类型绑定
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());

            //注册LitJson
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        }
    }
}
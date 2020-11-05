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

            //TODO:适配委托
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
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();

            //TODO:注册委托
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() =>
                {
                    ((Action)
                            action
                        )();
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>(
                    (a) =>
                    {
                        ((Action<float>) action
                            )(a);
                    });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<EventHandler<GameFramework.Event.GameEventArgs>>(
                (act) =>
                {
                    return new EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                    {
                        ((Action<object,
                            GameFramework.Event.GameEventArgs
                        >) act)(sender, e);
                    });
                });

            appdomain.DelegateManager.RegisterDelegateConvertor<EventHandler<ILTypeInstance>>((act) =>
            {
                return new
                    EventHandler<
                        ILTypeInstance
                    >((sender, e) =>
                    {
                        ((Action<
                            object,
                            ILTypeInstance
                        >) act)(
                            sender,
                            e);
                    });
            });


            appdomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetSuccessCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetSuccessCallback(
                        (assetName, asset, duration, userData) =>
                        {
                            ((Action<System.String, System.Object
                                  , System.Single, System.Object>)
                                act)(assetName, asset, duration,
                                     userData);
                        });
                });

            appdomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetFailureCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetFailureCallback(
                        (assetName, status, errorMessage, userData) =>
                        {
                            ((Action<System.String, GameFramework.Resource.LoadResourceStatus, System.String,
                                System.Object>) act)(assetName, status, errorMessage, userData);
                        });
                });

            appdomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
            {
                return new System.
                    AsyncCallback(
                        (ar) => { ((Action<System.IAsyncResult>) act)(ar); });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ThreadStart>((act) =>
            {
                return new System.
                    Threading.
                    ThreadStart(
                        () =>
                        {
                            ((Action)
                                    act
                                )();
                        });
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
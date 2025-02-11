﻿using System;
using System.Collections.Generic;
using Bing.Utils.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Bing.Dependency
{
    /// <summary>
    /// 应用程序服务定位器。可随时正常解析<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
    /// 如果当前处于HttpContext有效的范围内，可正常解析<see cref="ServiceLifetime.Scoped"/>的服务
    /// 注：服务定位器尚不能正常解析 RootServiceProvider.CreateScope() 生命周期内的 Scoped 的服务
    /// </summary>
    public sealed class ServiceLocator : IDisposable
    {
        /// <summary>
        /// 懒加载实例
        /// </summary>
        private static readonly Lazy<ServiceLocator> InstanceLazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        /// <summary>
        /// 服务提供程序
        /// </summary>
        private IServiceProvider _provider;

        /// <summary>
        /// 服务集合
        /// </summary>
        private IServiceCollection _services;

        /// <summary>
        /// 服务定位器实例
        /// </summary>
        public static ServiceLocator Instance => InstanceLazy.Value;

        /// <summary>
        /// ServiceProvider是否可用
        /// </summary>
        public bool IsProviderEnabled => _provider != null;

        /// <summary>
        /// <see cref="ServiceLifetime.Scoped"/>生命周期的服务提供者
        /// </summary>
        public IServiceProvider ScopedProvider
        {
            get
            {
                var scopeResolver = _provider.GetService<IScopeServiceResolver>();
                return scopeResolver != null && scopeResolver.ResolveEnabled ? scopeResolver.ScopeProvider : null;
            }
        }

        /// <summary>
        /// 初始化一个<see cref="ServiceLocator"/>类型的实例
        /// </summary>
        private ServiceLocator() { }

        /// <summary>
        /// 当前是否处于<see cref="ServiceLifetime.Scoped"/>生命周期中
        /// </summary>
        /// <returns></returns>
        public static bool InScoped() => Instance.ScopedProvider != null;

        /// <summary>
        /// 设置应用程序服务集合
        /// </summary>
        /// <param name="services">服务集合</param>
        internal void SetServiceCollection(IServiceCollection services)
        {
            Check.NotNull(services,nameof(services));
            _services = services;
        }

        /// <summary>
        /// 设置应用程序服务提供程序
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        internal void SetApplicationServiceProvider(IServiceProvider provider)
        {
            Check.NotNull(provider,nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// 获取所有已注册的<see cref="ServiceDescriptor"/>对象
        /// </summary>
        public IEnumerable<ServiceDescriptor> GetServiceDescriptors()
        {
            Check.NotNull(_services, nameof(_services));
            return _services;
        }

        /// <summary>
        /// 解析指定类型的服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        public T GetService<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            var scopedResolver = _provider.GetService<IScopeServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
                return scopedResolver.GetService<T>();
            return _provider.GetService<T>();
        }

        /// <summary>
        /// 解析指定类型的服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public object GetService(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            var scopedResolver = _provider.GetService<IScopeServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
                return scopedResolver.GetService(serviceType);
            return _provider.GetService(serviceType);
        }

        /// <summary>
        /// 解析指定类型的所有服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        public IEnumerable<T> GetServices<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            var scopedResolver = _provider.GetService<IScopeServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
                return scopedResolver.GetServices<T>();
            return _provider.GetServices<T>();
        }

        /// <summary>
        /// 解析指定类型的所有服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            var scopedResolver = _provider.GetService<IScopeServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
                return scopedResolver.GetServices(serviceType);
            return _provider.GetServices(serviceType);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _services = null;
            _provider = null;
        }
    }
}

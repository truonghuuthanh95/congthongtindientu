using CongThongTinDienTu.Repositories.Implements;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;

namespace CongThongTinDienTu.App_Start
{
    public static class IocConfigration
    {
        public static void ConfigrationIocContainer()
        {
            IUnityContainer container = new UnityContainer();
            registerService(container);
            DependencyResolver.SetResolver(new UnityResolvers(container));

        }

        private static void registerService(IUnityContainer container)
        {
            container.RegisterType<ISchoolRepository, SchoolRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDistrictRepository, DistrictRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IWardRepository, WardRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IHopDongRepository, HopDongRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICapTruongRepository, CapTruongRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAccountRepository, AccountRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAccountPermissionRepository, AccountPermissionRepository>(new HierarchicalLifetimeManager());
        }
    }
}
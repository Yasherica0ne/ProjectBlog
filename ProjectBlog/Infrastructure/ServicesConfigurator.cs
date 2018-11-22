using Microsoft.Extensions.DependencyInjection;
using ProjectBlog.ContentSearch.Repositories;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBlog.Infrastructure
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllersInCurrentAssembly();
            serviceCollection.AddSingleton<ICatalogRepository, CatalogRepository>();
        }
    }
}
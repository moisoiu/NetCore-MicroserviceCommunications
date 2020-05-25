using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Returns true if domain database is created
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static bool SetupDomainDatabase<T>(this IServiceCollection serviceCollection) where T : DbContext
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<T>();            

            if (context.Database.EnsureCreated())
            {
                return true;
            }
            return false;
        }
    }
}

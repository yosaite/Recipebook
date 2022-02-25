using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Recipebook.Models;
using System;

namespace Recipebook
{
    public enum RoleValue
    {
        User,
        Admin
    }
    public enum RecipeSort
    {
        Newest = 0,
        Oldest = 1,
        HighestRate = 2,
        LowestRate = 3
    }
    public static class Extensions
    {
        public const int Limit = 3;
        public static void RunAppSetup(this IServiceCollection services)
        {
            CreateRoles(services);
        }
        private static void CreateRoles(this IServiceCollection services)
        {
            foreach (var roleName in Enum.GetNames(typeof(RoleValue)))
            {
                CreateRole(services, roleName);
            }
        }
        private static async void CreateRole(IServiceCollection services, string roleName)
        {
            var serviceProvider = services.BuildServiceProvider();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}

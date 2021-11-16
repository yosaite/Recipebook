using Microsoft.AspNetCore.Identity;
using System;

namespace Recipebook.Models
{
    public enum RoleValue
    {
        User,
        Admin
    }
    public class Role : IdentityRole
    {
        public RoleValue RoleValue { get; set; }

        public Role(string name) : base(name)
        {
            if (Enum.TryParse(name, out RoleValue roleValue))
                RoleValue = roleValue;

        }
    }
}

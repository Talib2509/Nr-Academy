using NrAcademyCORE.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Extensions
{
    public static class RoleExtensions
    {
        public static string GetRole(this Roles role)
        {
            return role.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using Microsoft.AspNet.Identity;                 //RoleManager
using Microsoft.AspNet.Identity.EntityFramework; //IdentityRole, RoleStore
#endregion

namespace ChinookSystem.Security
{
    public class RoleManager:RoleManager<IdentityRole>
    {
        public RoleManager():
            base(new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {
                
        }

        //this method will be executed when the application starts up under IIS
        //(Internet Information Services)
        public void AddStartUpRoles()
        {
            foreach (string rolename in SecurtityRoles.StartupSecurityRoles)
            {
                //check if the role already exists in the security tables located in the database
                if (!Roles.Any(r => r.Name.Equals(rolename)))
                {
                    this.Create(new IdentityRole(rolename));
                }
            }
        } // end AddStartUpRoles
    }
}

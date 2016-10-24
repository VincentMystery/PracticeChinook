using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using Microsoft.AspNet.Identity;                 //RoleManager
using Microsoft.AspNet.Identity.EntityFramework; //IdentityRole, RoleStore
using System.ComponentModel;
#endregion

namespace ChinookSystem.Security
{
    [DataObject]
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
        } // End AddStartUpRoles

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<RoleProfile> ListAllRoles()
        {
            var um = new UserManager();

            //The data from roles needs to be in memory for use by the query --> use .ToList()
            var results = from role in Roles.ToList()
                          select new RoleProfile
                          {
                              RoleId = role.Id
                              ,
                              RoleName = role.Name
                              ,
                              UserNames = role.Users.Select(r => um.FindById(r.UserId).UserName)
                          };
            return results.ToList();
        }// End of ListAllRoles()

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddRole(RoleProfile role)
        {
            //Any Business roles to consider?
            //The role should not already exist on the roles table
            if (!this.RoleExists(role.RoleName))
            {
                this.Create(new IdentityRole(role.RoleName));
            }
        }// End of AddRole()

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveRole(RoleProfile role)
        {
            this.Delete(this.FindById(role.RoleId));
        } //End of RemoveRole()

        //This method will produce a list of all role names.
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<string> ListAllRoleNames()
        {
            return this.Roles.Select(r => r.Name).ToList();
        }

    }// End of class RoleManager:RoleManager<IdentityRole>
}// End of Namespace

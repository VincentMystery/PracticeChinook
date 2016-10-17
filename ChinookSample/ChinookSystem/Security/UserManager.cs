using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region additional namespaces
using Microsoft.AspNet.Identity.EntityFramework; //UserStore
using Microsoft.AspNet.Identity;                 //UserManager
#endregion

namespace ChinookSystem.Security
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }
        //Setting up the default webMaster
        #region Constants
        private const string STR_DEFAULT_PASSWORD = "Pa$$word1";
        private const string STR_USERNAME_FORMAT = "{0}.{1}";
        private const string STR_EMAIL_FORMAT = "{0}@chinook.ca";
        private const string STR_WEBMASTER_USERNAME = "WebMaster";
        #endregion

        public void AddWebmaster()
        {
            if (!Users.Any(u => u.UserName.Equals(STR_WEBMASTER_USERNAME)))
            {
                var webMasterAccount = new ApplicationUser()
                {
                    UserName = STR_WEBMASTER_USERNAME
                    , Email = string.Format(STR_EMAIL_FORMAT, STR_WEBMASTER_USERNAME)
                };

                //This command is from the inherited UserManager Class
                //This command creates a record on the security Users Table (AspNetUsers)
                this.Create(webMasterAccount, STR_DEFAULT_PASSWORD);

                //This AddToRole command is from the inherited user manager class,
                //creates a record on the security UserRole table (AspNetUserRole)
                this.AddToRole(webMasterAccount.Id, SecurtityRoles.WebsiteAdmins);
            }
        }

    }// End of UserManager
}//End of Namespace

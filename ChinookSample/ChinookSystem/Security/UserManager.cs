using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region additional namespaces
using Microsoft.AspNet.Identity.EntityFramework; //UserStore
using Microsoft.AspNet.Identity;                 //UserManager
using System.ComponentModel;                     //ODS
using ChinookSystem.DAL;                         //Context
#endregion

namespace ChinookSystem.Security
{
    [DataObject]
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
                this.AddToRole(webMasterAccount.Id, SecurityRoles.WebsiteAdmins);
            }
        }//End of AddWebmaster()

        //create CRUD methods for adding a user to the security User table
        //read of data to display on gridview
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<UnregisteredUserProfile> ListAllUnregisteredUsers()
        {
            using (var context = new ChinookContext())
            {
                //the data needs to be in memory for exicution by the NEXT query
                //to acomplish this use .ToList() which will force the query to execute.
                
                //List set containing a list of employeeIds
                var resisteredEmployees = (from emp in Users
                                          where emp.EmployeeId.HasValue
                                          select emp.EmployeeId).ToList();

                //compare the List() set to the user data table Employees
                var unregisteredEmployees = (from emp in context.Employees
                                            where !resisteredEmployees.Any(eid => emp.EmployeeId == eid)
                                            select new UnregisteredUserProfile
                                            {
                                                UserID = emp.EmployeeId
                                                ,
                                                FirstName = emp.FirstName
                                                ,
                                                LastName = emp.LastName
                                                ,
                                                UserType = UnRegisteredUserType.Employee
                                            }).ToList();

                //List() set containing a list of customers
                var resisteredCustomers = (from emp in Users
                                          where emp.EmployeeId.HasValue
                                          select emp.EmployeeId).ToList();

                //compare the List() set to the user data table Customers
                var unregisteredCustomers = (from cust in context.Customers
                                            where !resisteredCustomers.Any(cid => cust.CustomerId == cid)
                                            select new UnregisteredUserProfile
                                            {
                                                UserID = cust.CustomerId
                                                , FirstName = cust.FirstName
                                                , LastName = cust.LastName
                                                , UserType = UnRegisteredUserType.Customer
                                            }).ToList();


                //Combine the two physically identical layout datasets
                return unregisteredEmployees.Union(unregisteredCustomers).ToList();
            }
        }//End of ListAllUnregisteredUsers

        //register a user to the User Table (GridView)
        public void RegisterUser(UnregisteredUserProfile userinfo)
        {
            //basic info needed for the user record
            //password, email, username
            //you could randomly generate a password, we will use the default for now
            //the instance of the required user is based on our ApplicationUser
            var newUserAccount = new ApplicationUser()
            {
                UserName = userinfo.UserName,
                Email = userinfo.Email
            };

            //set the customerId or EmployeeId
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        newUserAccount.Id = userinfo.UserID.ToString();
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        newUserAccount.Id = userinfo.UserID.ToString();
                        break;
                    }
            }

            //create the actual AspNetUser Record
            this.Create(newUserAccount, STR_DEFAULT_PASSWORD);

            //assign user to an appropriate role
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
      //                  newUserAccount.Id = userinfo
                        this.AddToRole(newUserAccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
     //TODO                   this.AddToRole(newUserAccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }


        }//End of RegisterUser()

        //add a user to the User Table (ListView)


        //delete a user from the User Table (ListView)



    }// End of UserManager
}//End of Namespace

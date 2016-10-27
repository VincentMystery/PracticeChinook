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
using ChinookSystem.Data.Entities;               //Entity classes
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
                                                CustomerEmployeeId = emp.EmployeeId
                                                , FirstName = emp.FirstName
                                                , LastName = emp.LastName
                                                , UserType = UnRegisteredUserType.Employee
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
                                                CustomerEmployeeId = cust.CustomerId
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
                UserName = userinfo.AssignedUserName,
                Email = userinfo.AssignedEmail
            };

            //set the customerId or EmployeeId
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        newUserAccount.CustomerId = userinfo.CustomerEmployeeId;
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        newUserAccount.EmployeeId = userinfo.CustomerEmployeeId;
                        break;
                    }
            }

            //create the actual AspNetUser Record
            this.Create(newUserAccount, STR_DEFAULT_PASSWORD);

            //assign user to an appropriate role
            //uses the guid like user id from the User's table
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        this.AddToRole(newUserAccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        this.AddToRole(newUserAccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }


        }//End of RegisterUser()

        //list all current users
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UserProfile> ListAllUsers()
        {
            //we will be using the role manager to get roles
            var rm = new RoleManager();

            //get the current users off the User security table
            var results = from person in Users.ToList()
                          select new UserProfile()
                          {
                              UserId = person.Id
                              , UserName = person.UserName
                              , Email = person.Email
                              , EmailConfirmed = person.EmailConfirmed
                              , CustomerId = person.CustomerId
                              , EmployeeId = person.EmployeeId
                              , RoleMemberships = person.Roles.Select(r => rm.FindById(r.RoleId).Name)
                          };
            //using our own data tables gather the user firstname and lastname
            using (var context = new ChinookContext())
            {
                Employee eTemp;
                Customer cTemp;

                foreach (var person in results)
                {
                    if (person.EmployeeId.HasValue)
                    {
                        eTemp = context.Employees.Find(person.EmployeeId);
                        person.FirstName = eTemp.FirstName;
                        person.LastName = eTemp.LastName;
                    }
                    else if (person.CustomerId.HasValue)
                    {
                        cTemp = context.Customers.Find(person.CustomerId);
                        person.FirstName = cTemp.FirstName;
                        person.LastName = cTemp.LastName;
                    }
                    else
                    {
                        person.FirstName = "Unknown";
                        person.LastName = "";
                    }
                }
            }

            return results.ToList();

        }// End of ListAllUsers

        //add a user to the User Table (ListView)
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddUser(UserProfile userinfo)
        {
            //create an instance representing the new user
            var useraccount = new ApplicationUser()
            {
                UserName = userinfo.UserName
                ,
                Email = userinfo.Email
            };

            //create th enew user on the physical Usaers table
            this.Create(useraccount, STR_DEFAULT_PASSWORD);

            //create the UserRoles which were choosen at insert time
            foreach (var roleName in userinfo.RoleMemberships)
            {
                this.AddToRole(useraccount.Id, roleName);
            }
        }// End of AddUser()


        //delete a user from the User Table (ListView)
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveUser(UserProfile userinfo)
        {
            //Business Rules:
            //the webmaster cannot be deleted

            //realize the only info you have at this time is the
            //DataKeyNames value which is the User ID (on the User Security table the feild is id)

            //obtain the username from the security user table using the User ID value
            string username = this.Users
                              .Where(u => u.Id == userinfo.UserId)
                              .Select(u => u.UserName)
                              .SingleOrDefault()
                              .ToString();

            //remove user
            if (username.Equals(STR_WEBMASTER_USERNAME))
            {
                throw new Exception("The Webmaster account cannot be removed.");
            }
            else
            {
                this.Delete(this.FindById(userinfo.UserId));
            }

        }// End of RemoveUser()


    }// End of UserManager
}//End of Namespace

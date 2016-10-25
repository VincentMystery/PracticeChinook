using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    public enum UnRegisteredUserType { Undefined, Employee, Customer }
    public class UnregisteredUserProfile
    {
        public int UserID { get; set; } //generated
        public string UserName { get; set; } //collected
        public string FirstName { get; set; } //comes from user table
        public string LastName { get; set; } //comes from user table
        public string Email { get; set; } //collected
        public UnRegisteredUserType UserType { get; set; } 
    }
}

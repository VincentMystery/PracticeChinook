using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel;    //ODS
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
#endregion


namespace ChinookSystem.BLL
{
    [DataObject]
    public class CustomerController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RepresentitveCustomers> RepresentiveCustomers_Get(int employeeId)
        {
            using (var context = new ChinookContext())
            {
                var results =
                    from a in context.Customers
                    where a.SupportRepId == employeeId
                    select new RepresentitveCustomers
                    {
                        Name = a.LastName + ", " + a.FirstName
                        , City = a.City
                        , State = a.State
                        , Phone = a.Phone
                        , Email = a.Email
                    };

                return results.ToList();
            }
        }
    }
}

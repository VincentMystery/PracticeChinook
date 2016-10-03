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
    public class AlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AlbumYearList> AlbumYearList_Get(int year)
        {
            using (var context = new ChinookContext())
            {
                var results = from a in context.Albums
                              orderby a.ReleaseYear
                              select new AlbumYearList
                              {
                                  year = a.ReleaseYear
                              };
                return results.ToList();
            }
        }
    }
}

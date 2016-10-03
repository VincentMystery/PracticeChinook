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
    public class ArtistController
    {
        //Dump the entire Artist entity
        //this will use Entity Framework access
        //Entityt classes will be used to define the data
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Artist> Artist_ListAll()
        {
            //set up transaction area
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }

        //Report a datset containing data from multiple entities
        //this will use Linq to entity access
        //POCO classes will be used to define the data
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbums> ArtistAlbum_Get(int year)
        {

            //when you bring your query form linqPad to your program
            //

            //set up transaction area

            //you may also need to change your navigation referencing use in LinqPad
            //to the navigation properties you stated in the Entity class definitions.

            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ReleaseYear == year
                              orderby x.Artists.Name, x.Title
                              select new ArtistAlbums
                              {
                                  //naem and title are POCO class property names
                                 Name = x.Artists.Name
                                 , Title = x.Title
                              };
                //The following requires the query data in memory  .ToList()
                //At this point the query will actually execute.
                return results.ToList();
            }
        }

    }
}

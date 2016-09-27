using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Data.Entity;
using ChinookSystem.Data.Entities;
#endregion

namespace ChinookSystem.DAL
{
    //internal for security reason
    //access restricted to within the component library
    //inherit DbContext for entity framework requires
    //      System.Data.Entity
    internal class ChinookContext : DbContext
    {
        //public ChinookContext():base("ChinookDB");
        public ChinookContext() : base("ChinookDB") { }
            //setup DbSet prooperties
            public DbSet<Artist> Artists { get; set; }
            public DbSet<Album> Albums { get; set; }
            public DbSet<Track> Tracks { get; set; }
            public DbSet<MediaType> MediaTypes { get; set; }
            
    
    }
}

    


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Adittional Namespaces
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace ChinookSystem.Data.Entities
{
    //point to the SQL table that this file maps
    [Table("MediaTypes")]
    public class MediaType
    {
        //Key notations is option if the sql pkey ends in ID or Id
        //Required if default of entity is NOT Identity
        //Required if pkey is compound

        //properties can be fully implemented or auto implemented
        //property names should use sql attribute name
        //properties should be listed in the same order as the sql attributes for easy maintenance

        [Key]
        public int MediaTypeId { get; set; }
        public string? Name { get; set; }

        //Navigation properties for use by Linq
        //these properties will be of type virtual
        //There are two types of navigation properties
        //properties that point to "children" use ICollection<T>
        //properties that point to "Parent" use ParentName as the data type
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
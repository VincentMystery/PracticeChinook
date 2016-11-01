using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additonal Namespaces
using System.ComponentModel; //ODS
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class TrackController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Track> ListTracks()
        {
            using (var context = new ChinookContext())
            {
                //return all records and attributes
                return context.Tracks.ToList();
            }
        }// End of ListTracks

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Track GetTrack(int trackid)
        {
            using (var context = new ChinookContext())
            {
                //return a record and attributes
                return context.Tracks.Find(trackid);
            }
        }// End of GetTrack

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddTrack(Track trackInfo)
        {
            using (var context = new ChinookContext())
            {
                //any business rules
                if (trackInfo.UnitPrice > 1.0m)
                    throw new Exception("Bob's Your Uncle.");

                //any data refinements
                //review of using iif
                //the composer can be a null string,
                //we do not wish to store an empty string
                trackInfo.Composer = string.IsNullOrEmpty(trackInfo.Composer) ?
                                        null : trackInfo.Composer;

                //add the instance of trackinfo to the database
                context.Tracks.Add(trackInfo);

                //Commits the add
                context.SaveChanges();

            }
        }// End of AddTrack

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateTrack(Track trackInfo)
        {
            using (var context = new ChinookContext())
            {
                //any business rules


                //any data refinements
                //review of using iif
                //the composer can be a null string,
                //we do not wish to store an empty string
                trackInfo.Composer = string.IsNullOrEmpty(trackInfo.Composer) ?
                                        null : trackInfo.Composer;

                //Update the existing instance of trackinfo on the database
                context.Entry(trackInfo).State = System.Data.Entity.EntityState.Modified;

                //Commits the update
                context.SaveChanges();

            }
        }// End of UpdateTrack

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeleteTrack(Track trackInfo)
        {
            DeleteTrack(trackInfo.TrackId);
        }

        public void DeleteTrack(int trackID)
        {
            using (var context = new ChinookContext())
            {
                //business rules

                //do the delete
                //find existing record on the database
                var existing = context.Tracks.Find(trackID);

                //delete record form database
                context.Tracks.Remove(existing);

                //commits transaction
                context.SaveChanges();
            }
        }

    }// End of TrackController
}

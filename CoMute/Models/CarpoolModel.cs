using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoMute.Web.Models
{
    public class CarpoolModel
    {
        public int Carpool_ID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Leader { get; set; }
        public string DaysAvailable { get; set; }
        public int AvailbaleSeats { get; set; }
        public int User_ID { get; set; }
        public System.TimeSpan DepartureTime { get; set; }
        public System.TimeSpan ArrivalTime { get; set; }
        public string Notes { get; set; }
        public Nullable<int> NumberOfUsers { get; set; }
    }
}
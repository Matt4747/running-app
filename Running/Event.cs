using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Running
{
    [Table("event")]
    public class Event
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }

        public string Date { get; set; }
        public double Distance { get; set; }
        public string Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Distance, Date, Time, ID);
        }
    }

}

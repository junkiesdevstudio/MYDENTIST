using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYDENTIST.Class.DatabaseHelper.Misc
{
    public class Status
    {
        private double differenceSeconds = double.NaN;

        private cds_BaseKonektor xCon = null;

        /// <summary>
        /// Returns server timestamp. Only needs to query the database once.
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerDateTime()
        {
            if (double.IsNaN(differenceSeconds))
                differenceSeconds = ((DateTime)xCon.GetObject("SELECT CURRENT_TIMESTAMP") - DateTime.Now).TotalSeconds;

            return DateTime.Now.AddSeconds(differenceSeconds);
        }

        public Status(cds_BaseKonektor xCon)
        {
            this.xCon = xCon;
        }
    }
}

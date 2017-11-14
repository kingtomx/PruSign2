using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public class DeviceLog : IEntity
    {
        //Interface Implementation
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Device { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string ErrorLocation { get; set; }
        public DateTime LogDate { get; set; }

        public string FormattedDate
        {
            get => LogDate.ToString("yyyy-MM-dd HH:mm:ss");
            set {
                value = LogDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
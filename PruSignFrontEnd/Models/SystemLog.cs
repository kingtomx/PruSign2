using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignFrontEnd.Models
{
    public class SystemLog
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        [Display(Name = "Error Location")]
        public string ErrorLocation { get; set; }

        [Display(Name = "Stack Trace")]
        public string StackTrace { get; set; }

        [Display(Name = "Created Date")]
        public string FormattedCreateDate
        {
            get => Created.ToString("yyyy-MM-dd HH:mm:ss");
            set { }
        }
    }
}
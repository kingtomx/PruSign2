using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignFrontEnd.Models
{
    public class Signature
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }

        [Display(Name = "Document ID")]
        public string DocumentId { get; set; }

        [Display(Name = "Application ID")]
        public string ApplicationId { get; set; }

        public string SignatureObject { get; set; }

        [Display(Name = "Created Date")]
        public string FormattedCreateDate
        {
            get => Created.ToString("yyyy-MM-dd HH:mm:ss");
            set { }
        }

        [Display(Name = "Updated Date")]
        public string FormattedUpdateDate
        {
            get => Updated.ToString("yyyy-MM-dd HH:mm:ss");
            set { }
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.ViewModels
{
    public class DeviceQueryViewModel
    {
        [Display(Name = "Question Code")]
        public string QuestionCode { get; set; }

        [Display(Name = "Question")]
        public string QuestionDescription { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Response")]
        public string Response { get; set; }
    }
}
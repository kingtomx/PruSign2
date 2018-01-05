using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Entities
{
    public class Queries
    {
        public string QuestionCode { get; set; }
        public string QuestionDescription { get; set; }
        public string Response { get; set; }
        public bool Sent { get; set; }
    }
}

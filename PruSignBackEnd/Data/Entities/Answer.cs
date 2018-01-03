using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public enum Status
    {
        WaitingForAnswer,
        Done
    }

    public class Answer : IEntity
    {
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Status Status { get; set; }
        public Question Question { get; set; }
        public string Device { get; set; }
        public string Data { get; set; }
    }
}
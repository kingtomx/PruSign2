using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public class Answer : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Question Question { get; set; }
        public string Device { get; set; }

        public enum Status
        {
            WaitingForAnswer,
            Done
        }

        public string Data { get; set; }
    }
}
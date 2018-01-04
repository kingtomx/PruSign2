using System;
using System.ComponentModel.DataAnnotations;

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
        public virtual Question Question { get; set; }
        public int QuestionID  { get; set; }
        public virtual Device Device { get; set; }
        public int DeviceID { get; set; }
        public string Data { get; set; }
    }
}
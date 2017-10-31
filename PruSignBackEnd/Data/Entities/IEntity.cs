using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public interface IEntity
    {
        [Key]
        int ID { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
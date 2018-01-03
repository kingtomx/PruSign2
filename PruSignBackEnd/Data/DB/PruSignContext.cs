using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.DB
{
    public class PruSignContext : DbContext
    {
        public PruSignContext()
            : base("PruSignDatabase")
        {
        }

        public DbSet<Signature> Signatures { get; set; }
        public DbSet<PointWhen> PointsWhen { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<DeviceLog> DeviceLogs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
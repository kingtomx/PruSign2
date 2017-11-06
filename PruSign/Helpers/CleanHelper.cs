using PruSign.Data;
using PruSign.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Helpers
{
    public static class CleanHelper
    {
        public static async void CleanOldLogs()
        {
            var db = new PruSignDatabase();
            var serviceLogs = new ServiceAsync<LogEntry>(db);
            var logs = await serviceLogs.GetAll().Where(l => (DateTime.Now - l.SentDate).TotalDays >= 30).ToListAsync();

            foreach (var l in logs)
            {
                await serviceLogs.Delete(l);
            }
        }

        public static async void CleanSentSignatures()
        {
            var db = new PruSignDatabase();
            var serviceSignatures = new ServiceAsync<Signature>(db);
            var signatures = await serviceSignatures.GetAll().Where(s => s.Sent).ToListAsync();

            foreach (var s in signatures)
            {
                await serviceSignatures.Delete(s);
            }
        }
    }
}

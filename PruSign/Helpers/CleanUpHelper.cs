using PruSign.Data;
using PruSign.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Helpers
{
    public static class CleanUpHelper
    {
        public static async void CleanOldLogs()
        {
            try
            {
                var db = new PruSignDatabase();
                var serviceLogs = new ServiceAsync<LogEntry>(db);
                var today = DateTime.Now;
                var logs = await serviceLogs.GetAll().ToListAsync();

                foreach (var l in logs)
                {
                    if ((DateTime.Now - l.SentDate).TotalDays >= 30)
                    {
                        await serviceLogs.Delete(l);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }

        }

        public static async void CleanSentSignatures()
        {
            try
            {
                var db = new PruSignDatabase();
                var serviceSignatures = new ServiceAsync<Signature>(db);
                var signatures = await serviceSignatures.GetAll().Where(s => s.Sent).ToListAsync();

                foreach (var s in signatures)
                {
                    await serviceSignatures.Delete(s);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }
        }
    }
}

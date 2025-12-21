using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChameRozAP.ServiceManager
{
    public class ChameRozManager
    {
        public void Start()
        {


            if (IsTodayChameAvailable())
            {

                return;

            }
            else
            {

                while (!IsInternetAvailable())
                {
                    ShowMessageAP.ShowMessageBoxAP("The internet is off, turn it on.", "ChameRozAP");

                    Thread.Sleep(300000);
                }


                GitManager gitManager = new GitManager();
                DataBaseManager dbM = new DataBaseManager();
                var chameYesterdays = dbM.GetYesterdaysChame();
                var ChameToday = dbM.GetTodayChame();
                gitManager.add_history_chame_commit(YesterdaysChame: chameYesterdays, TodayChame: ChameToday);
            }
        }

        private bool IsTodayChameAvailable()
        {
            using var db = new CL_DBcontextAP();
            return db.ChameRozToday.Any(c => c.DateTime.Date == DateTime.Today);
        }

        public bool IsInternetAvailable()
        {
            try
            {
                using var ping = new Ping();
                var reply = ping.Send("8.8.8.8", 3000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}

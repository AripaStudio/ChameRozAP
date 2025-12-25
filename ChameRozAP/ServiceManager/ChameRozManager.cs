using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;
using Timer = System.Timers.Timer;

namespace ChameRozAP.ServiceManager
{
    public class ChameRozManager
    {
        private Timer timer = new Timer();
        public void Start()
        {


            if (IsTodayChameAvailable())
            {
                var TimeRemaining = GetTimeRemainingUntil();
                ShowMessageAP.ShowMessageBoxAP("A poem has been posted for today, the next poem in time: Hours : " + TimeRemaining.Hours + " Minutes : " + TimeRemaining.Minutes, "ChameRozAP");
                timer.Interval = TimeRemaining.TotalMilliseconds;
                timer.AutoReset = false;
                timer.Elapsed += TimerOnElapsed;
                timer.Start();

            }
            else
            {
                StartWork();
            }



            

        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            if (sender is Timer t)
            {
                t.Stop();
                t.Elapsed -= TimerOnElapsed;
                t.Dispose();
            }
            ShowMessageAP.ShowMessageBoxAP("A new poem was posted.", "ChameRozAP");
            StartWork();
        }

        private void StartWork()
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

        private bool IsTodayChameAvailable()
        {
            using var db = new CL_DBcontextAP();
            return db.ChameRozToday.Any(c => c.DateTime.Date == DateTime.Today);
        }

        private bool IsInternetAvailable()
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


        private TimeSpan GetTimeRemainingUntil()
        {
            var GetNowDate = DateTime.Now;
            var tomrrow = GetNowDate.Date.AddDays(1);
            return tomrrow - GetNowDate;
        }



    }
}

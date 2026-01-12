using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ChameRozAP.ServiceManager
{
    public class GitManager
    {

        //کامل کردن این بخش و تست کردن این بخش ها تا زمانی که نت ها درست بشه 
        private string? PathProject;
        public GitManager()
        {
            PathProject = Environment.GetEnvironmentVariable("ChameRozPathProject");
        }

        public void add_history_chame_commit(dbInfoTodayChame YesterdaysChame, dbInfoTodayChame TodayChame , bool IsInternetValid)
        {
            CheckPastPoem();
            AddHistory(YesterdaysChame);
            AddNewChame_commit(TodayChame);
            if (IsInternetValid)
            {
                ExecuteCommand("git push");
            }
            else
            {
                ShowMessageAP.ShowMessageBoxAP("The internet is off, turn it on.", "ChameRozAP");

                Thread.Sleep(300000);
            }
            
        }

        

        public void CheckPastPoem()
        {
            string HistoryFolderAddress = CreateHistoryFolderAndGetAddress();

            var LastPoem = AripaTools.GetNameAndTimeSpanOfLastCreatedFiles(HistoryFolderAddress);
            int Days = LastPoem.TimeSpanFile.Days - 1;
            if (Days < 0)
            {
                for (int i = 0; i < Days; i++)
                {
                    DataBaseManager dbM = new DataBaseManager();
                    var PastDateTime = DateTime.Now.Date.AddDays(-i);
                    AddHistory(dbM.CreateNewChame(PastDateTime));

                }
            }
        }

        private string CreateHistoryFolderAndGetAddress()
        {
            var historyFolderPath = Path.Combine(PathProject, "History");
            if (!Directory.Exists(historyFolderPath))
            {
                Directory.CreateDirectory(historyFolderPath);
            }
            return historyFolderPath;
        }
        
        private void AddHistory(dbInfoTodayChame? YesterdaysChame)
        {
            if (string.IsNullOrEmpty(PathProject))
            {
                return;
            }
           
            if (YesterdaysChame == null || YesterdaysChame.chameRoz == null)
            {

                return;
            }
          
            var fileName = $"{YesterdaysChame.DateTime:yyyy-MM-dd}.md";
            var newFile = Path.Combine(CreateHistoryFolderAndGetAddress(), fileName);
            string textReadme = $@"# 🖋️ چامه امروز
> هر روز چامه نو بر این جا گذاشته می‌شود. برای دیدن چامه‌های گذشته به پوشه **History** بروید.

---

### {YesterdaysChame.chameRoz.PoetName}
{YesterdaysChame.chameRoz.ChameText.Replace("\n", "  \n")}

---
**📅 تاریخ:** {YesterdaysChame.DateTime:yyyy-MM-dd}";

            File.WriteAllText(newFile, textReadme);

            ExecuteCommand("git add .");
            ExecuteCommand("git commit -m \"Update History Chame\"");

        }
        private void AddNewChame_commit(dbInfoTodayChame todayChame)
        {
            if (string.IsNullOrEmpty(PathProject))
            {
                return;
            }

            var readmePath = Path.Combine(PathProject, "README.md");
            if (!File.Exists(readmePath))
            {
                return;
            }

            string textReadme = $@"# 🖋️ چامه امروز
> هر روز چامه نو بر این جا گذاشته می‌شود. برای دیدن چامه‌های گذشته به پوشه **History** بروید.

---

### {todayChame.chameRoz.PoetName}
{todayChame.chameRoz.ChameText.Replace("\n", "  \n")}

---
**📅 تاریخ:** {todayChame.DateTime:yyyy-MM-dd}";
            File.WriteAllText(readmePath, textReadme);

            ExecuteCommand("git add .");
            ExecuteCommand("git commit -m \"Update Daily Chame\"");

        }

        private void ExecuteCommand(string command)
        {

            if (string.IsNullOrEmpty(PathProject))
            {
                return;
            }

            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command)
            {
                WorkingDirectory = PathProject,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            using var process = System.Diagnostics.Process.Start(processInfo);
            process?.WaitForExit();
        }
    }
}

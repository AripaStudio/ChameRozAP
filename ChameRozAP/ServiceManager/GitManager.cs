using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameRozAP.ServiceManager
{
    public class GitManager
    {
        private string? PathProject;
        public GitManager()
        {
            PathProject = Environment.GetEnvironmentVariable("ChameRozPathProject");
        }

        public void add_history_chame_commit(dbInfoTodayChame YesterdaysChame, dbInfoTodayChame TodayChame)
        {
            AddHistory(YesterdaysChame);
            AddNewChame_commit(TodayChame);
            ExecuteCommand("git push");
        }

        private void AddHistory(dbInfoTodayChame YesterdaysChame)
        {
            if (string.IsNullOrEmpty(PathProject))
            {
                return;
            }

            var historyFolderPath = Path.Combine(PathProject, "History");
            if (!Directory.Exists(historyFolderPath))
            {
                Directory.CreateDirectory(historyFolderPath);
            }
            var fileName = $"{YesterdaysChame.DateTime:yyyy-MM-dd}.md";
            var newFile = Path.Combine(historyFolderPath, fileName);
            File.WriteAllText(newFile, YesterdaysChame.chameRoz.ChameText + Environment.NewLine + YesterdaysChame.chameRoz.PoetName);

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

            string TextReadme = "#چامه امروز - هر روز چامه نو بر این جا گذاشته میشود - برای دیدن چامه های گذشته به پوشه history بروید." + Environment.NewLine + todayChame.chameRoz.ChameText + Environment.NewLine + todayChame.chameRoz.PoetName + Environment.NewLine + todayChame.DateTime.ToLongDateString();
            File.WriteAllText(readmePath, TextReadme);

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

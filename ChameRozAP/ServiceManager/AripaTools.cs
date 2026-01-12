using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace ChameRozAP.ServiceManager
{
    public static  class AripaTools
    {
        public struct NameAndTimeSpanInfoFile
        {
            public string NameFile;
            public TimeSpan TimeSpanFile;
        }
        public static NameAndTimeSpanInfoFile GetNameAndTimeSpanOfLastCreatedFiles(string FolderPath)
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                return new NameAndTimeSpanInfoFile();
            }
            var todayDateTime = DateTime.Now;

            string[] PathAllFile = Directory.GetFiles(FolderPath);
            Dictionary<string, TimeSpan> SaveTimesCreatedFiles = new Dictionary<string, TimeSpan>();
            foreach (string s in PathAllFile)
            {
                var FileCreatedDateTime = File.GetCreationTime(s);
                SaveTimesCreatedFiles.Add(s , FileCreatedDateTime - todayDateTime);
            }

            var TimeSpanLastFile = SaveTimesCreatedFiles.Values.Max();
            var NameFileLastFile = SaveTimesCreatedFiles.Keys.Max();
            return new NameAndTimeSpanInfoFile()
            {
                NameFile = NameFileLastFile,
                TimeSpanFile = TimeSpanLastFile,
            };
        }
    }

   
}

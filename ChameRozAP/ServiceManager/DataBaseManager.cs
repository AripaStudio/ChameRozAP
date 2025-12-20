using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameRozAP.ServiceManager
{
    public class DataBaseManager
    {
        public dbInfoTodayChame GetTodayChame()
        {
            using var db = new CL_DBcontextAP();
            db.Database.EnsureCreated();


            var totalCount = db.ChameRoz.Count();
            var random = new Random();
            var skip = random.Next(0, totalCount);


            var selectedChame = db.ChameRoz
                .OrderBy(c => c.ChameID)
                .Skip(skip)
                .Take(1)
                .FirstOrDefault();

            int nextId = db.ChameRozToday.Any() ? db.ChameRozToday.Max(i => i.id) + 1 : 1;

            var NewInfoTodayChame = new dbInfoTodayChame
            {
                id = nextId,
                chameRoz = selectedChame,
                DateTime = DateTime.Now
            };
            db.ChameRozToday.Add(NewInfoTodayChame);
            db.SaveChanges();
            return NewInfoTodayChame;

        }

        public dbInfoTodayChame GetYesterdaysChame()
        {
            using var db = new CL_DBcontextAP();
            db.Database.EnsureCreated();


            var lastPoem = db.ChameRozToday
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (lastPoem == null)
            {
                return new dbInfoTodayChame();
            }

            return lastPoem;

        }

        public string GetTextTodayChame(dbInfoTodayChame selectedChameRoz)
        {
            return selectedChameRoz.chameRoz.ChameText;
        }

    }

    public class dbChameRoz
    {
        [Key]
        public int ChameID { get; set; }

        public string PoetName { get; set; }

        public string CategoryName { get; set; }

        public string ChameText { get; set; }

    }

    public class dbInfoTodayChame
    {
        [Key]
        public int id { get; set; }

        public int ChameID { get; set; }

        [ForeignKey("ChameID")]
        public dbChameRoz chameRoz { get; set; }

        public DateTime DateTime { get; set; }

    }

    public class CL_DBcontextAP : DbContext
    {
        public DbSet<dbChameRoz> ChameRoz { get; set; }

        public DbSet<dbInfoTodayChame> ChameRozToday { get; set; }

        public CL_DBcontextAP()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=ChameRozAPdb.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

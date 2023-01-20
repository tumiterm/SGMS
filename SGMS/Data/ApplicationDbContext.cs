using Microsoft.EntityFrameworkCore;
using SGMS.Models;
using SGMS.DTO;

namespace SGMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PlayerSelection> Selections { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SysUsers> EmergencyUsers { get; set; }










    }

}

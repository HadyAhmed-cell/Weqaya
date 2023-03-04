using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VirtualClinic.Entities;

namespace VirtualClinic.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<GenderType> Genders { get; set; }
        public DbSet<SocialStatusType> SocialStatus { get; set; }
        public DbSet<SyndicatesTypes> Syndicates { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Lab> Labs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
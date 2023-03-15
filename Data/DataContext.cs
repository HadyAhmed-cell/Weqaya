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

        public DbSet<TestsAndRisks> testsAndRisks { get; set; }
        public DbSet<SocialStatusType> SocialStatus { get; set; }
        public DbSet<SyndicatesTypes> Syndicates { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabPatient> LabPatients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<LabPatient>()
            .HasKey(lp => new { lp.LabId, lp.PatientId });

            modelBuilder.Entity<LabPatient>()
                .HasOne(lp => lp.Lab)
                .WithMany(l => l.LabPatients)
                .HasForeignKey(lp => lp.LabId);

            modelBuilder.Entity<LabPatient>()
                .HasOne(lp => lp.Patient)
                .WithMany(p => p.labPatients)
                .HasForeignKey(lp => lp.PatientId);

            modelBuilder.Entity<DoctorPatient>()
            .HasKey(lp => new { lp.doctorId, lp.patientId });

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(lp => lp.doctor)
                .WithMany(l => l.doctorPatients)
                .HasForeignKey(lp => lp.doctorId);

            modelBuilder.Entity<DoctorPatient>()
                 .HasOne(lp => lp.patient)
                 .WithMany(l => l.doctorPatients)
                 .HasForeignKey(lp => lp.patientId);
        }
    }
}
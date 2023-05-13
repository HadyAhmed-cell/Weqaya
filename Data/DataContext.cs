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
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabPatient> LabPatients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<PatientTestsOrRisks> PatientTestsAndRisks { get; set; }
        public DbSet<LabsTestsAndRisks> LabsTestsAndRisks { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

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

            modelBuilder.Entity<PatientTestsOrRisks>(e =>
            {
                e.HasKey(lp => new { lp.PatientId, lp.TestTestsAndRisksId });

                e.HasOne(lp => lp.Patient)
                .WithMany(lp => lp.PatientTestsAndRisks)
                .HasForeignKey(lp => lp.PatientId);

                e.HasOne(lp => lp.TestsAndRisks)
                .WithMany(lp => lp.PatientTestsOrRisks)
                .HasForeignKey(lp => lp.TestTestsAndRisksId);
            });
            modelBuilder.Entity<LabsTestsAndRisks>(e =>
            {
                e.HasKey(lp => new { lp.LabId, lp.TestsAndRisksId });

                e.HasOne(lp => lp.Lab)
                .WithMany(lp => lp.LabsTestsAndRisks)
                .HasForeignKey(lp => lp.LabId);

                e.HasOne(lp => lp.TestsAndRisks)
                .WithMany(lp => lp.LabsTestsAndRisks)
                .HasForeignKey(lp => lp.TestsAndRisksId);
            });

            modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId);
            modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Reviews)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId);
            modelBuilder.Entity<Lab>()
            .HasMany(d => d.Reviews)
            .WithOne(a => a.Lab)
            .HasForeignKey(a => a.LabId);
        }
    }
}
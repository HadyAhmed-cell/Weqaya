﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtualClinic.Data;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    [DbContext(typeof(DataContext))]
    [Migration("20230606102809_ConvertingBackToInt")]
    partial class ConvertingBackToInt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VirtualClinic.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AppointmentDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DoctorInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Education")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Speciality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubSpeciatlity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TimeFrom")
                        .HasColumnType("int");

                    b.Property<int?>("TimeTo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DoctorNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusNum")
                        .HasColumnType("int");

                    b.Property<int>("doctorId")
                        .HasColumnType("int");

                    b.Property<int>("patientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DoctorHistories");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorPatient", b =>
                {
                    b.Property<int>("doctorId")
                        .HasColumnType("int");

                    b.Property<int>("patientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DoctorNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusNum")
                        .HasColumnType("int");

                    b.HasKey("doctorId", "patientId");

                    b.HasIndex("patientId");

                    b.ToTable("DoctorPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorReviews", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("Reviews")
                        .HasColumnType("int");

                    b.Property<string>("ReviewsComments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DoctorId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("DoctorReviews");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabDescript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Labs");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Results")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusNum")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<int>("labId")
                        .HasColumnType("int");

                    b.Property<int>("patientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LabHistories");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabPatient", b =>
                {
                    b.Property<int>("LabId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Results")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusNum")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.HasKey("LabId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("LabPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabReviews", b =>
                {
                    b.Property<int>("LabId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("Reviews")
                        .HasColumnType("int");

                    b.Property<string>("ReviewsComments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LabId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("LabReviews");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabsTestsAndRisks", b =>
                {
                    b.Property<int>("LabId")
                        .HasColumnType("int");

                    b.Property<int>("TestsAndRisksId")
                        .HasColumnType("int");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.HasKey("LabId", "TestsAndRisksId");

                    b.HasIndex("TestsAndRisksId");

                    b.ToTable("LabsTestsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Diabetes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiabetesRelatives")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<string>("HighPressure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicineForDiabetesOrPressure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelativesWithHeartAttacksOrHighColestrol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Smoking")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.PatientTestsOrRisks", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("TestTestsAndRisksId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "TestTestsAndRisksId");

                    b.HasIndex("TestTestsAndRisksId");

                    b.ToTable("PatientTestsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.PatientTestsOrRisksOcr", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("TestTestsAndRisksId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "TestTestsAndRisksId");

                    b.HasIndex("TestTestsAndRisksId");

                    b.ToTable("PatientTestsOrRisksOcrs");
                });

            modelBuilder.Entity("VirtualClinic.Entities.TestsAndRisks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TestsOrRisks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("testsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Appointment", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorPatient", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Doctor", "doctor")
                        .WithMany("doctorPatients")
                        .HasForeignKey("doctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.Patient", "patient")
                        .WithMany("doctorPatients")
                        .HasForeignKey("patientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("doctor");

                    b.Navigation("patient");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorReviews", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Doctor", "Doctor")
                        .WithMany("Reviews")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.Patient", "Patient")
                        .WithMany("DoctorReviews")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabPatient", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Lab", "Lab")
                        .WithMany("LabPatients")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.Patient", "Patient")
                        .WithMany("labPatients")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabReviews", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Lab", "Lab")
                        .WithMany("Reviews")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.Patient", "Patient")
                        .WithMany("LabReviews")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabsTestsAndRisks", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Lab", "Lab")
                        .WithMany("LabsTestsAndRisks")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.TestsAndRisks", "TestsAndRisks")
                        .WithMany("LabsTestsAndRisks")
                        .HasForeignKey("TestsAndRisksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("TestsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.PatientTestsOrRisks", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Patient", "Patient")
                        .WithMany("PatientTestsAndRisks")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.TestsAndRisks", "TestsAndRisks")
                        .WithMany("PatientTestsOrRisks")
                        .HasForeignKey("TestTestsAndRisksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("TestsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.PatientTestsOrRisksOcr", b =>
                {
                    b.HasOne("VirtualClinic.Entities.Patient", "Patient")
                        .WithMany("PatientTestsOrRisksOcr")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualClinic.Entities.TestsAndRisks", "TestsAndRisks")
                        .WithMany("PatientTestsOrRisksOcr")
                        .HasForeignKey("TestTestsAndRisksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("TestsAndRisks");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Reviews");

                    b.Navigation("doctorPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Lab", b =>
                {
                    b.Navigation("LabPatients");

                    b.Navigation("LabsTestsAndRisks");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Patient", b =>
                {
                    b.Navigation("DoctorReviews");

                    b.Navigation("LabReviews");

                    b.Navigation("PatientTestsAndRisks");

                    b.Navigation("PatientTestsOrRisksOcr");

                    b.Navigation("doctorPatients");

                    b.Navigation("labPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.TestsAndRisks", b =>
                {
                    b.Navigation("LabsTestsAndRisks");

                    b.Navigation("PatientTestsOrRisks");

                    b.Navigation("PatientTestsOrRisksOcr");
                });
#pragma warning restore 612, 618
        }
    }
}

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
    [Migration("20230424060305_AddingLabResults")]
    partial class AddingLabResults
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VirtualClinic.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AppoinmentsDatesAvailable")
                        .HasColumnType("datetime2");

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DoctorInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PHD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<byte[]>("ProfilePhoto")
                        .HasColumnType("varbinary(max)");

                    b.Property<double>("Reviews")
                        .HasColumnType("float");

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("VirtualClinic.Entities.DoctorPatient", b =>
                {
                    b.Property<int>("doctorId")
                        .HasColumnType("int");

                    b.Property<int>("patientId")
                        .HasColumnType("int");

                    b.HasKey("doctorId", "patientId");

                    b.HasIndex("patientId");

                    b.ToTable("DoctorPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabDescript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Reviews")
                        .HasColumnType("float");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Labs");
                });

            modelBuilder.Entity("VirtualClinic.Entities.LabPatient", b =>
                {
                    b.Property<int>("LabId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Results")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LabId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("LabPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Diabetes")
                        .HasColumnType("bit");

                    b.Property<bool?>("DiabetesRelatives")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool?>("HighPressure")
                        .HasColumnType("bit");

                    b.Property<string>("LabResults")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("MedicineForDiabetesOrPressure")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NoOfKids")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("RelativesWithHeartAttacksOrHighColestrol")
                        .HasColumnType("bit");

                    b.Property<bool?>("Smoking")
                        .HasColumnType("bit");

                    b.Property<string>("SocialStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Syndicates")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WaistDiameter")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
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

            modelBuilder.Entity("VirtualClinic.Entities.Doctor", b =>
                {
                    b.Navigation("doctorPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Lab", b =>
                {
                    b.Navigation("LabPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.Patient", b =>
                {
                    b.Navigation("PatientTestsAndRisks");

                    b.Navigation("doctorPatients");

                    b.Navigation("labPatients");
                });

            modelBuilder.Entity("VirtualClinic.Entities.TestsAndRisks", b =>
                {
                    b.Navigation("PatientTestsOrRisks");
                });
#pragma warning restore 612, 618
        }
    }
}

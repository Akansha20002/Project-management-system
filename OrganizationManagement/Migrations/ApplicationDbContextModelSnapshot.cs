﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OrganizationManagement.DBContext;

#nullable disable

namespace OrganizationManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OrganizationManagement.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdminId")
                        .HasColumnType("integer");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProjectId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProjectId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("IsAutomated")
                        .HasColumnType("boolean");

                    b.Property<string>("Steps")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TestSuiteId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TestSuiteId");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestPlan", b =>
                {
                    b.Property<int>("TestPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TestPlanId"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Objective")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<string>("Strategy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TestPlanId");

                    b.HasIndex("ProjectId");

                    b.ToTable("TestsPlans");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ActualResult")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExpectedResult")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TestCaseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestCaseId");

                    b.ToTable("TestSteps");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestSuite", b =>
                {
                    b.Property<int>("TestSuiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TestSuiteId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TestPlanId")
                        .HasColumnType("integer");

                    b.HasKey("TestSuiteId");

                    b.HasIndex("TestPlanId");

                    b.ToTable("TestSuites");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Organization", b =>
                {
                    b.HasOne("OrganizationManagement.Models.Admin", null)
                        .WithMany("Organizations")
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Project", b =>
                {
                    b.HasOne("OrganizationManagement.Models.Organization", null)
                        .WithMany("Projects")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestCase", b =>
                {
                    b.HasOne("OrganizationManagement.Models.TestSuite", null)
                        .WithMany("TestCases")
                        .HasForeignKey("TestSuiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestPlan", b =>
                {
                    b.HasOne("OrganizationManagement.Models.Project", null)
                        .WithMany("TestPlans")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestStep", b =>
                {
                    b.HasOne("OrganizationManagement.Models.TestCase", null)
                        .WithMany("TestSteps")
                        .HasForeignKey("TestCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestSuite", b =>
                {
                    b.HasOne("OrganizationManagement.Models.TestPlan", null)
                        .WithMany("TestSuites")
                        .HasForeignKey("TestPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationManagement.Models.Admin", b =>
                {
                    b.Navigation("Organizations");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Organization", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("OrganizationManagement.Models.Project", b =>
                {
                    b.Navigation("TestPlans");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestCase", b =>
                {
                    b.Navigation("TestSteps");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestPlan", b =>
                {
                    b.Navigation("TestSuites");
                });

            modelBuilder.Entity("OrganizationManagement.Models.TestSuite", b =>
                {
                    b.Navigation("TestCases");
                });
#pragma warning restore 612, 618
        }
    }
}

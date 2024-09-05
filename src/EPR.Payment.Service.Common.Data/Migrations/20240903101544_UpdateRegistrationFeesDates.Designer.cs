﻿// <auto-generated />
using System;
using EPR.Payment.Service.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240903101544_UpdateRegistrationFeesDates")]
    partial class UpdateRegistrationFeesDates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Group", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Producer Type",
                            Type = "ProducerType"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Compliance Scheme",
                            Type = "ComplianceScheme"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Producer Subsidiaries",
                            Type = "ProducerSubsidiaries"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Compliance Scheme Subsidiaries",
                            Type = "ComplianceSchemeSubsidiaries"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Producer re-submitting a report",
                            Type = "ProducerResubmission"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("PaymentStatus", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Status = "Initiated"
                        },
                        new
                        {
                            Id = 1,
                            Status = "InProgress"
                        },
                        new
                        {
                            Id = 2,
                            Status = "Success"
                        },
                        new
                        {
                            Id = 3,
                            Status = "Failed"
                        },
                        new
                        {
                            Id = 4,
                            Status = "Error"
                        },
                        new
                        {
                            Id = 5,
                            Status = "UserCancelled"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.RegistrationFees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("RegulatorId")
                        .HasColumnType("int");

                    b.Property<int>("SubGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RegulatorId");

                    b.HasIndex("SubGroupId");

                    b.ToTable("RegistrationFees", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 262000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 1,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 2,
                            Amount = 262000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 2,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 3,
                            Amount = 262000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 3,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 4,
                            Amount = 262000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 4,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 5,
                            Amount = 121600m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 1,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 6,
                            Amount = 121600m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 2,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 7,
                            Amount = 121600m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 3,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 8,
                            Amount = 121600m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 1,
                            RegulatorId = 4,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 9,
                            Amount = 165800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 1,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 10,
                            Amount = 165800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 2,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 11,
                            Amount = 165800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 3,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 12,
                            Amount = 165800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 4,
                            SubGroupId = 1
                        },
                        new
                        {
                            Id = 13,
                            Amount = 63100m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 1,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 14,
                            Amount = 63100m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 2,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 15,
                            Amount = 63100m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 3,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 16,
                            Amount = 63100m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 4,
                            SubGroupId = 2
                        },
                        new
                        {
                            Id = 17,
                            Amount = 1380400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 1,
                            SubGroupId = 3
                        },
                        new
                        {
                            Id = 18,
                            Amount = 1380400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 2,
                            SubGroupId = 3
                        },
                        new
                        {
                            Id = 19,
                            Amount = 1380400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 3,
                            SubGroupId = 3
                        },
                        new
                        {
                            Id = 20,
                            Amount = 1380400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 4,
                            SubGroupId = 3
                        },
                        new
                        {
                            Id = 21,
                            Amount = 257900m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 1,
                            SubGroupId = 4
                        },
                        new
                        {
                            Id = 22,
                            Amount = 257900m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 2,
                            SubGroupId = 4
                        },
                        new
                        {
                            Id = 23,
                            Amount = 257900m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 3,
                            SubGroupId = 4
                        },
                        new
                        {
                            Id = 24,
                            Amount = 257900m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 2,
                            RegulatorId = 4,
                            SubGroupId = 4
                        },
                        new
                        {
                            Id = 25,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 1,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 26,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 2,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 27,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 3,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 28,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 4,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 29,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 1,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 30,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 2,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 31,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 3,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 32,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 3,
                            RegulatorId = 4,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 33,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 1,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 34,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 2,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 35,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 3,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 36,
                            Amount = 55800m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 4,
                            SubGroupId = 5
                        },
                        new
                        {
                            Id = 37,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 1,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 38,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 2,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 39,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 3,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 40,
                            Amount = 14000m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 4,
                            RegulatorId = 4,
                            SubGroupId = 6
                        },
                        new
                        {
                            Id = 41,
                            Amount = 71400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 5,
                            RegulatorId = 1,
                            SubGroupId = 7
                        },
                        new
                        {
                            Id = 42,
                            Amount = 71400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 5,
                            RegulatorId = 2,
                            SubGroupId = 7
                        },
                        new
                        {
                            Id = 43,
                            Amount = 71400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 5,
                            RegulatorId = 3,
                            SubGroupId = 7
                        },
                        new
                        {
                            Id = 44,
                            Amount = 71400m,
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            EffectiveTo = new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc),
                            GroupId = 5,
                            RegulatorId = 4,
                            SubGroupId = 7
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.Regulator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Regulator", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "England",
                            Type = "GB-ENG"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Scotland",
                            Type = "GB-SCT"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Wales",
                            Type = "GB-WLS"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Northern Ireland",
                            Type = "GB-NIR"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.SubGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("SubGroup", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Large producer",
                            Type = "Large"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Small producer",
                            Type = "Small"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Registration",
                            Type = "Registration"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Online Market",
                            Type = "Online"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Up to 20",
                            Type = "UpTo20"
                        },
                        new
                        {
                            Id = 6,
                            Description = "More than 20",
                            Type = "MoreThan20"
                        },
                        new
                        {
                            Id = 7,
                            Description = "Re-submitting a report",
                            Type = "ReSubmitting"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)")
                        .HasColumnOrder(12);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(14);

                    b.Property<string>("ErrorCode")
                        .HasColumnType("varchar(255)")
                        .HasColumnOrder(9);

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("varchar(255)")
                        .HasColumnOrder(10);

                    b.Property<Guid>("ExternalPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(4)
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("GovPayStatus")
                        .HasColumnType("varchar(20)")
                        .HasColumnOrder(8);

                    b.Property<string>("GovpayPaymentId")
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(5);

                    b.Property<int>("InternalStatusId")
                        .HasColumnType("int")
                        .HasColumnOrder(6);

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(3);

                    b.Property<string>("ReasonForPayment")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnOrder(13);

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnOrder(11);

                    b.Property<string>("Regulator")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnOrder(7);

                    b.Property<Guid>("UpdatedByOrganisationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(16);

                    b.Property<Guid>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(15);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(17);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("ExternalPaymentId")
                        .IsUnique();

                    b.HasIndex("GovpayPaymentId")
                        .IsUnique()
                        .HasFilter("[GovpayPaymentId] IS NOT NULL");

                    b.HasIndex("InternalStatusId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.RegistrationFees", b =>
                {
                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.Group", "Group")
                        .WithMany("RegistrationFees")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.Regulator", "Regulator")
                        .WithMany("RegistrationFees")
                        .HasForeignKey("RegulatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.SubGroup", "SubGroup")
                        .WithMany("RegistrationFees")
                        .HasForeignKey("SubGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Regulator");

                    b.Navigation("SubGroup");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Payment", b =>
                {
                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", "PaymentStatus")
                        .WithMany("Payments")
                        .HasForeignKey("InternalStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.Group", b =>
                {
                    b.Navigation("RegistrationFees");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.Regulator", b =>
                {
                    b.Navigation("RegistrationFees");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.SubGroup", b =>
                {
                    b.Navigation("RegistrationFees");
                });
#pragma warning restore 612, 618
        }
    }
}

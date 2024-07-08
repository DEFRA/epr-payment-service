﻿// <auto-generated />
using System;
using EPR.Payment.Service.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.AdditionalFees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("FeesSubType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("AdditionalFees", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 714m,
                            Country = "GB-ENG",
                            Description = "Resubmission",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Resub"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 714m,
                            Country = "GB-SCT",
                            Description = "Resubmission",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Resub"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 714m,
                            Country = "GB-WLS",
                            Description = "Resubmission",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Resub"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 714m,
                            Country = "GB-NIR",
                            Description = "Resubmission",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Resub"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 332m,
                            Country = "GB-ENG",
                            Description = "Late",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Late"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 332m,
                            Country = "GB-SCT",
                            Description = "Late",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Late"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 332m,
                            Country = "GB-WLS",
                            Description = "Late",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Late"
                        },
                        new
                        {
                            Id = 8,
                            Amount = 332m,
                            Country = "GB-NIR",
                            Description = "Late",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesSubType = "Late"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.ComplianceShemeRegitrationFees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("FeesType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("ComplianceShemeRegitrationFees", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 13804m,
                            Country = "GB-ENG",
                            Description = "Registration",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "Reg"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 13804m,
                            Country = "GB-SCT",
                            Description = "Registration",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "Reg"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 13804m,
                            Country = "GB-WLS",
                            Description = "Registration",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "Reg"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 13804m,
                            Country = "GB-NIR",
                            Description = "Registration",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "Reg"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 1658m,
                            Country = "GB-ENG",
                            Description = "Large Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "L"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 1658m,
                            Country = "GB-SCT",
                            Description = "Large Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "L"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 1658m,
                            Country = "GB-WLS",
                            Description = "Large Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "L"
                        },
                        new
                        {
                            Id = 8,
                            Amount = 1658m,
                            Country = "GB-NIR",
                            Description = "Large Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "L"
                        },
                        new
                        {
                            Id = 9,
                            Amount = 631m,
                            Country = "GB-ENG",
                            Description = "Small Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "S"
                        },
                        new
                        {
                            Id = 10,
                            Amount = 631m,
                            Country = "GB-SCT",
                            Description = "Small Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "S"
                        },
                        new
                        {
                            Id = 11,
                            Amount = 631m,
                            Country = "GB-WLS",
                            Description = "Small Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "S"
                        },
                        new
                        {
                            Id = 12,
                            Amount = 631m,
                            Country = "GB-NIR",
                            Description = "Small Producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "S"
                        },
                        new
                        {
                            Id = 13,
                            Amount = 2579m,
                            Country = "GB-ENG",
                            Description = "Online Market",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "On"
                        },
                        new
                        {
                            Id = 14,
                            Amount = 2579m,
                            Country = "GB-SCT",
                            Description = "Online Market",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "On"
                        },
                        new
                        {
                            Id = 15,
                            Amount = 2579m,
                            Country = "GB-WLS",
                            Description = "Online Market",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "On"
                        },
                        new
                        {
                            Id = 16,
                            Amount = 2579m,
                            Country = "GB-NIR",
                            Description = "Online Market",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FeesType = "On"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.InternalError", b =>
                {
                    b.Property<string>("InternalErrorCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ErrorMessage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("GovPayErrorCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("GovPayErrorMessage")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("InternalErrorCode");

                    b.ToTable("InternalError", "Lookup");

                    b.HasData(
                        new
                        {
                            InternalErrorCode = "A",
                            GovPayErrorCode = "P0030",
                            GovPayErrorMessage = "Cancelled"
                        },
                        new
                        {
                            InternalErrorCode = "B",
                            GovPayErrorCode = "P0020",
                            GovPayErrorMessage = "Expired"
                        },
                        new
                        {
                            InternalErrorCode = "C",
                            GovPayErrorCode = "P0010",
                            GovPayErrorMessage = "Rejected"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

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

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.ProducerRegitrationFees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProducerType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("ProducerRegitrationFees", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 2620m,
                            Country = "GB-ENG",
                            Description = "Large producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "L"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 2620m,
                            Country = "GB-SCT",
                            Description = "Large producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "L"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 2620m,
                            Country = "GB-WLS",
                            Description = "Large producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "L"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 2620m,
                            Country = "GB-NIR",
                            Description = "Large producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "L"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 1216m,
                            Country = "GB-ENG",
                            Description = "Small producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "S"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 1216m,
                            Country = "GB-SCT",
                            Description = "Small producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "S"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 1216m,
                            Country = "GB-WLS",
                            Description = "Small producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "S"
                        },
                        new
                        {
                            Id = 8,
                            Amount = 1216m,
                            Country = "GB-NIR",
                            Description = "Small producer",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProducerType = "S"
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.Subsidiaries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxSub")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int>("MinSub")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Subsidiaries", "Lookup");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 558m,
                            Country = "GB-ENG",
                            Description = "Up to 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 20,
                            MinSub = 1
                        },
                        new
                        {
                            Id = 2,
                            Amount = 558m,
                            Country = "GB-SCT",
                            Description = "Up to 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 20,
                            MinSub = 1
                        },
                        new
                        {
                            Id = 3,
                            Amount = 558m,
                            Country = "GB-WLS",
                            Description = "Up to 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 20,
                            MinSub = 1
                        },
                        new
                        {
                            Id = 4,
                            Amount = 558m,
                            Country = "GB-NIR",
                            Description = "Up to 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 20,
                            MinSub = 1
                        },
                        new
                        {
                            Id = 5,
                            Amount = 140m,
                            Country = "GB-ENG",
                            Description = "More then 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 100,
                            MinSub = 21
                        },
                        new
                        {
                            Id = 6,
                            Amount = 140m,
                            Country = "GB-SCT",
                            Description = "More then 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 100,
                            MinSub = 21
                        },
                        new
                        {
                            Id = 7,
                            Amount = 140m,
                            Country = "GB-WLS",
                            Description = "More then 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 100,
                            MinSub = 21
                        },
                        new
                        {
                            Id = 8,
                            Amount = 140m,
                            Country = "GB-NIR",
                            Description = "More then 20",
                            EffectiveFrom = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EffectiveTo = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxSub = 100,
                            MinSub = 21
                        });
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GovPayStatus")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("GovpayPaymentId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("InternalErrorCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("InternalStatusId")
                        .HasColumnType("int");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReasonForPayment")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Regulator")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("UpdatedByOrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GovpayPaymentId")
                        .IsUnique()
                        .HasFilter("[GovpayPaymentId] IS NOT NULL");

                    b.HasIndex("InternalErrorCode");

                    b.HasIndex("InternalStatusId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Payment", b =>
                {
                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.InternalError", "InternalError")
                        .WithMany("Payments")
                        .HasForeignKey("InternalErrorCode");

                    b.HasOne("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", "PaymentStatus")
                        .WithMany("Payments")
                        .HasForeignKey("InternalStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InternalError");

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.InternalError", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("EPR.Payment.Service.Common.Data.DataModels.Lookups.PaymentStatus", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}

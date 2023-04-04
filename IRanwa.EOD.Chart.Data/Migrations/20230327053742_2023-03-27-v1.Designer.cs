﻿// <auto-generated />
using System;
using IRanwa.EOD.Chart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    [DbContext(typeof(EODDBContext))]
    [Migration("20230327053742_2023-03-27-v1")]
    partial class _20230327v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("JoinNewsletter")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserImageContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.EODData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double?>("AdjustedClose")
                        .HasColumnType("float");

                    b.Property<double?>("Close")
                        .HasColumnType("float");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("ExchangeSymbol")
                        .HasColumnType("int");

                    b.Property<double?>("High")
                        .HasColumnType("float");

                    b.Property<double?>("Low")
                        .HasColumnType("float");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<double?>("Open")
                        .HasColumnType("float");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSymbol");

                    b.HasIndex("Timestamp");

                    b.ToTable("EODData");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.EODLiveData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double?>("AdjustedClose")
                        .HasColumnType("float");

                    b.Property<double?>("Close")
                        .HasColumnType("float");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("ExchangeSymbol")
                        .HasColumnType("int");

                    b.Property<double?>("High")
                        .HasColumnType("float");

                    b.Property<double?>("Low")
                        .HasColumnType("float");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<double?>("Open")
                        .HasColumnType("float");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSymbol");

                    b.ToTable("EODLiveData");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.ExchangeCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryISO2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryISO3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatingMIC")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.ToTable("ExchangeCodes");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.ExchangeSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AnnualSyncCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DataSyncCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Exchange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExchangeCodeId")
                        .HasColumnType("int");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("QuarterlySyncCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("SyncException")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("ExchangeCodeId");

                    b.ToTable("ExchangeSymbols");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.StockAnnual", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AssetsTurnOverRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CashRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("CurrentRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToAssets")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToEBITDA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToEquity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DividendYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBITDAInterestExpense")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBITDAMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EarningYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvEBITDA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvOpeningCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvSales")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExchangeSymbol")
                        .HasColumnType("int");

                    b.Property<string>("FreeCashFlowYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GrossProfitMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InterestCoverage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketCap")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("NetIncomeMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatingProfitMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PBRate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PERate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayoutRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToFreeCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToSale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuickRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReturnOnCapitalEmployed")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSymbol");

                    b.ToTable("StockAnnual");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.StockQuarterly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AssetsTurnOverRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CashRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("CurrentRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToAssets")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToEBITDA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitToEquity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DividendYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBITDAInterestExpense")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBITDAMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EarningYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvEBITDA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvOpeningCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvSales")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExchangeSymbol")
                        .HasColumnType("int");

                    b.Property<string>("FreeCashFlowYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GrossProfitMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InterestCoverage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketCap")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedUser")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("NetIncomeMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatingProfitMargin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PBRate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PERate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayoutRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToFreeCashFlow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceToSale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuickRatio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ROIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReturnOnCapitalEmployed")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSymbol");

                    b.ToTable("StockQuarterly");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.EODData", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ExchangeSymbol", "ExchangeSymbolData")
                        .WithMany()
                        .HasForeignKey("ExchangeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeSymbolData");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.EODLiveData", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ExchangeSymbol", "ExchangeSymbolData")
                        .WithMany()
                        .HasForeignKey("ExchangeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeSymbolData");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.ExchangeSymbol", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ExchangeCode", "ExchangeCodeModel")
                        .WithMany()
                        .HasForeignKey("ExchangeCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeCodeModel");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.StockAnnual", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ExchangeSymbol", "ExchangeSymbolData")
                        .WithMany()
                        .HasForeignKey("ExchangeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeSymbolData");
                });

            modelBuilder.Entity("IRanwa.EOD.Chart.Data.StockQuarterly", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ExchangeSymbol", "ExchangeSymbolData")
                        .WithMany()
                        .HasForeignKey("ExchangeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeSymbolData");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IRanwa.EOD.Chart.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("IRanwa.EOD.Chart.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

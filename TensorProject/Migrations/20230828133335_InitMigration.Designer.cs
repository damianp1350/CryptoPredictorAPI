﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace TensorProject.Migrations
{
    [DbContext(typeof(BinanceDbContext))]
    [Migration("20230828133335_InitMigration")]
    partial class InitMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TensorProject.Models.BinanceKlineModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("CloseTime")
                        .HasColumnType("bigint");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("NumberOfTrades")
                        .HasColumnType("int");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("OpenTime")
                        .HasColumnType("bigint");

                    b.Property<decimal>("QuoteAssetVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TakerBuyBaseAssetVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TakerBuyQuoteAssetVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Unused")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("BinanceHistoricalDatas");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    partial class ProjectDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Persistence.Entities.CurrentSimulationStep", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("SimulationStep")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CurrentSimulationSteps");
                });

            modelBuilder.Entity("Persistence.Entities.Stock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Drift")
                        .HasColumnType("REAL");

                    b.Property<float>("StartingPrice")
                        .HasColumnType("REAL");

                    b.Property<Guid>("StockId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<float>("Volatility")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("StockId")
                        .IsUnique();

                    b.HasIndex("Ticker")
                        .IsUnique();

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Persistence.Entities.StockPrice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Price")
                        .HasColumnType("REAL");

                    b.Property<long>("SimulationStep")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StockId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("StockId", "SimulationStep");

                    b.HasIndex("SimulationStep");

                    b.HasIndex("StockId");

                    b.ToTable("StockPrices");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreasureHunt.Infrastructure.Data;

#nullable disable

namespace TreasureHunt.API.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TreasureHunt.Core.Models.TreasureMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("M")
                        .HasColumnType("int");

                    b.Property<string>("MatrixData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("N")
                        .HasColumnType("int");

                    b.Property<int>("P")
                        .HasColumnType("int");

                    b.Property<double>("Result")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("TreasureMaps");
                });
#pragma warning restore 612, 618
        }
    }
}

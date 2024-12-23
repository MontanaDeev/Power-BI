﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerBI.Data;

#nullable disable

namespace PowerBI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241027082316_MakeCategoryNullable")]
    partial class MakeCategoryNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PowerBI.Models.Category", b =>
                {
                    b.Property<int>("idCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("idCategory"));

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("idCategory");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PowerBI.Models.Report", b =>
                {
                    b.Property<int>("idReport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("idReport"));

                    b.Property<int?>("CategoryidCategory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int?>("idCategory")
                        .HasColumnType("int");

                    b.Property<string>("publicLink")
                        .HasColumnType("longtext");

                    b.HasKey("idReport");

                    b.HasIndex("CategoryidCategory");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("PowerBI.Models.Report", b =>
                {
                    b.HasOne("PowerBI.Models.Category", "Category")
                        .WithMany("Reports")
                        .HasForeignKey("CategoryidCategory");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PowerBI.Models.Category", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}

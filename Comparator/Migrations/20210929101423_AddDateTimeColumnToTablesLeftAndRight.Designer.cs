﻿// <auto-generated />
using System;
using Comparator.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Comparator.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20210929101423_AddDateTimeColumnToTablesLeftAndRight")]
    partial class AddDateTimeColumnToTablesLeftAndRight
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Comparator.Models.Diff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int>("Offset")
                        .HasColumnType("int");

                    b.Property<int>("ResultId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ResultId");

                    b.ToTable("Diffs");
                });

            modelBuilder.Entity("Comparator.Models.Left", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Lefts");
                });

            modelBuilder.Entity("Comparator.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ComparationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("LeftId")
                        .HasColumnType("int");

                    b.Property<int>("ResultType")
                        .HasColumnType("int");

                    b.Property<int>("RightId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeftId");

                    b.HasIndex("RightId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Comparator.Models.Right", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("Comparator.Models.Diff", b =>
                {
                    b.HasOne("Comparator.Models.Result", "Result")
                        .WithMany("Diffs")
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Result");
                });

            modelBuilder.Entity("Comparator.Models.Result", b =>
                {
                    b.HasOne("Comparator.Models.Left", "Left")
                        .WithMany("Results")
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Comparator.Models.Right", "Right")
                        .WithMany("Results")
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Left");

                    b.Navigation("Right");
                });

            modelBuilder.Entity("Comparator.Models.Left", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("Comparator.Models.Result", b =>
                {
                    b.Navigation("Diffs");
                });

            modelBuilder.Entity("Comparator.Models.Right", b =>
                {
                    b.Navigation("Results");
                });
#pragma warning restore 612, 618
        }
    }
}

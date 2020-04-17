﻿// <auto-generated />
using System;
using ChallengePad.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChallengePad.Migrations
{
    [DbContext(typeof(ChallengePadDbContext))]
    partial class ChallengePadDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ChallengePad.Models.Objective", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OperationId")
                        .HasColumnType("bigint");

                    b.Property<long>("Points")
                        .HasColumnType("bigint");

                    b.Property<bool>("Solved")
                        .HasColumnType("boolean");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("OperationId");

                    b.ToTable("Objectives");
                });

            modelBuilder.Entity("ChallengePad.Models.Operation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("ChallengePad.Models.UploadedFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ObjectiveId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OperationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ObjectiveId");

                    b.HasIndex("OperationId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ChallengePad.Models.Objective", b =>
                {
                    b.HasOne("ChallengePad.Models.Operation", null)
                        .WithMany("Objectives")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChallengePad.Models.UploadedFile", b =>
                {
                    b.HasOne("ChallengePad.Models.Objective", null)
                        .WithMany("Files")
                        .HasForeignKey("ObjectiveId");

                    b.HasOne("ChallengePad.Models.Operation", null)
                        .WithMany("Files")
                        .HasForeignKey("OperationId");
                });
#pragma warning restore 612, 618
        }
    }
}

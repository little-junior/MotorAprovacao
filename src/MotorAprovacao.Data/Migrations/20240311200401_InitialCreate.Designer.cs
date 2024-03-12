﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotorAprovacao.Data.EF;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotorAprovacao.WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240311200401_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MotorAprovacao.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("character varying(70)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MotorAprovacao.Models.Entities.CategoryRules", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<decimal>("MaximumToApprove")
                        .HasMaxLength(10000)
                        .HasColumnType("numeric");

                    b.Property<decimal>("MinimumToDisapprove")
                        .HasMaxLength(10000)
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId")
                        .IsUnique();

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("MotorAprovacao.Models.Entities.RefundDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StatusDeterminedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Total")
                        .HasPrecision(5)
                        .HasColumnType("numeric(5)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("RefundDocuments");
                });

            modelBuilder.Entity("MotorAprovacao.Models.Entities.CategoryRules", b =>
                {
                    b.HasOne("MotorAprovacao.Models.Entities.Category", "Category")
                        .WithOne("CategoryRules")
                        .HasForeignKey("MotorAprovacao.Models.Entities.CategoryRules", "CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MotorAprovacao.Models.Entities.RefundDocument", b =>
                {
                    b.HasOne("MotorAprovacao.Models.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MotorAprovacao.Models.Entities.Category", b =>
                {
                    b.Navigation("CategoryRules")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

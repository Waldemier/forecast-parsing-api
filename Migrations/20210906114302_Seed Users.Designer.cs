﻿// <auto-generated />
using System;
using ForecastAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ForecastAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210906114302_Seed Users")]
    partial class SeedUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ForecastAPI.Data.Entities.History", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("History");
                });

            modelBuilder.Entity("ForecastAPI.Data.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ForecastAPI.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                            Email = "admin@brainence.com",
                            Name = "Admin",
                            Password = "$2a$11$C4ET8f6NfM/t7eJJ8hG8deSGsx/4y3B8ut7Onv4ID11hWOUziUQy6",
                            Role = 0
                        },
                        new
                        {
                            Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                            Email = "admin2@brainence.com",
                            Name = "Admin2",
                            Password = "$2a$11$AHeCsg0BcSkFbG8Nge4aA.c8mSmSncftTmpCGK7kN4QtUg7aq6lc.",
                            Role = 0
                        },
                        new
                        {
                            Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                            Email = "mark@brainence.com",
                            Name = "Mark",
                            Password = "$2a$11$ubNfpuPaWrXYTSbG7H05n.e70WfFE9kYCxF6KbZzcJAg4DlfMxmb2",
                            Role = 1
                        },
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Email = "iryna@brainence.com",
                            Name = "Iryna",
                            Password = "$2a$11$YRTE221fcY/XGTI5LnEaKu/a6cHHdyew0PlH/6OjN8f1x/DOzofTG",
                            Role = 1
                        });
                });

            modelBuilder.Entity("ForecastAPI.Data.Entities.RefreshToken", b =>
                {
                    b.HasOne("ForecastAPI.Data.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("ForecastAPI.Data.Entities.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
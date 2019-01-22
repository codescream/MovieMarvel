﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieMarvel.Models;

namespace MovieMarvel.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20180830184625_Hurrah")]
    partial class Hurrah
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieMarvel.Models.Cart", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Closed");

                    b.Property<DateTime>("Opened");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<int>("UserID");

                    b.HasKey("id");

                    b.HasIndex("UserID");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("MovieMarvel.Models.Item", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CartID");

                    b.Property<float>("Cost");

                    b.Property<int>("Duration");

                    b.Property<string>("MovieID")
                        .IsRequired();

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<int?>("Userid");

                    b.HasKey("id");

                    b.HasIndex("CartID");

                    b.HasIndex("Userid");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("MovieMarvel.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("status")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MovieMarvel.Models.Vote", b =>
                {
                    b.Property<int>("VoteID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MovieID");

                    b.Property<int>("VoteRating");

                    b.HasKey("VoteID");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("MovieMarvel.Models.Cart", b =>
                {
                    b.HasOne("MovieMarvel.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieMarvel.Models.Item", b =>
                {
                    b.HasOne("MovieMarvel.Models.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieMarvel.Models.User")
                        .WithMany("Items")
                        .HasForeignKey("Userid");
                });
#pragma warning restore 612, 618
        }
    }
}

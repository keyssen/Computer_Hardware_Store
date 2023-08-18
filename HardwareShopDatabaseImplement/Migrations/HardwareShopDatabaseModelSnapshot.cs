﻿// <auto-generated />
using System;
using HardwareShopDatabaseImplement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HardwareShopDatabaseImplement.Migrations
{
    [DbContext(typeof(HardwareShopDatabase))]
    partial class HardwareShopDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.ComponentBuild", b =>
                {
                    b.Property<int>("ComponentId")
                        .HasColumnType("integer");

                    b.Property<int>("BuildId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("ComponentId", "BuildId");

                    b.HasIndex("BuildId");

                    b.ToTable("ComponentsBuilds");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.GoodComponent", b =>
                {
                    b.Property<int>("GoodId")
                        .HasColumnType("integer");

                    b.Property<int>("ComponentId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("GoodId", "ComponentId");

                    b.HasIndex("ComponentId");

                    b.ToTable("GoodsComponents");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.PurchaseBuild", b =>
                {
                    b.Property<int>("PurchaseId")
                        .HasColumnType("integer");

                    b.Property<int>("BuildId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("PurchaseId", "BuildId");

                    b.HasIndex("BuildId");

                    b.ToTable("PurchasesBuilds");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.PurchaseGood", b =>
                {
                    b.Property<int>("PurchaseId")
                        .HasColumnType("integer");

                    b.Property<int>("GoodId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("PurchaseId", "GoodId");

                    b.HasIndex("GoodId");

                    b.ToTable("PurchasesGoods");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ComponentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Cost")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Good", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("GoodName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateImplement")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GoodId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<double>("Sum")
                        .HasColumnType("double precision");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Build", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BuildName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Builds");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BuildId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DatePurchase")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PurchaseStatus")
                        .HasColumnType("integer");

                    b.Property<double>("Sum")
                        .HasColumnType("double precision");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.ComponentBuild", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Worker.Build", "Build")
                        .WithMany("Components")
                        .HasForeignKey("BuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.Storekeeper.Component", "Component")
                        .WithMany("Builds")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Build");

                    b.Navigation("Component");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.GoodComponent", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Storekeeper.Component", "Component")
                        .WithMany("Goods")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.Storekeeper.Good", "Good")
                        .WithMany("Components")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("Good");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.PurchaseBuild", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Worker.Build", "Build")
                        .WithMany("Purchases")
                        .HasForeignKey("BuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.Worker.Purchase", "Purchase")
                        .WithMany("Builds")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Build");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.ManyToMany.PurchaseGood", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Storekeeper.Good", "Good")
                        .WithMany("Purchases")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.Worker.Purchase", "Purchase")
                        .WithMany("Goods")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Component", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Components")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Good", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Goods")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Order", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Storekeeper.Good", "Good")
                        .WithMany("Orders")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Build", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Builds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Comment", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.Worker.Build", "Build")
                        .WithMany("Comments")
                        .HasForeignKey("BuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Build");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Purchase", b =>
                {
                    b.HasOne("HardwareShopDatabaseImplement.Models.User", null)
                        .WithMany("Purchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Component", b =>
                {
                    b.Navigation("Builds");

                    b.Navigation("Goods");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Storekeeper.Good", b =>
                {
                    b.Navigation("Components");

                    b.Navigation("Orders");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.User", b =>
                {
                    b.Navigation("Builds");

                    b.Navigation("Comments");

                    b.Navigation("Components");

                    b.Navigation("Goods");

                    b.Navigation("Orders");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Build", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Components");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("HardwareShopDatabaseImplement.Models.Worker.Purchase", b =>
                {
                    b.Navigation("Builds");

                    b.Navigation("Goods");
                });
#pragma warning restore 612, 618
        }
    }
}
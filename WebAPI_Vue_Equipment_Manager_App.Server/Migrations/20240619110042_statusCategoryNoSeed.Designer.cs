﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20240619110042_statusCategoryNoSeed")]
    partial class statusCategoryNoSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("Depth")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int?>("Height")
                        .HasColumnType("integer");

                    b.Property<int?>("Length")
                        .HasColumnType("integer");

                    b.Property<int?>("ListPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Model_Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Model_Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModelCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("EquipmentTypes");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Centrifuge"
                        });
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModelDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ModelId")
                        .HasColumnType("integer");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("ModelDocuments");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Date_Of_Acceptance_Test")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Date_Of_Activation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Date_Of_Reciept")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ItemStatusCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("LocalName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("ModelId")
                        .HasColumnType("integer");

                    b.Property<bool?>("New_On_Reciept")
                        .HasColumnType("boolean");

                    b.Property<int?>("Purchase_Order")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Purchase_Price")
                        .HasColumnType("numeric");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UnitId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemStatusCategoryId");

                    b.HasIndex("ModelId");

                    b.HasIndex("UnitId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemDocuments");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("ItemNotes");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemStatusCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ItemStatusCategory");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Maintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date_Completed")
                        .HasColumnType("date");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Provider_Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Maintenances");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.MaintenanceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MaintenanceTypes");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.MaintenanceFrequency", b =>
                {
                    b.Property<int>("ModelId")
                        .HasColumnType("integer");

                    b.Property<int>("UnitId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("Frquency")
                        .HasColumnType("integer");

                    b.HasKey("ModelId", "UnitId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("MaintenanceFrequencys");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Building")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<string>("Room")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Units");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.UserAssignment", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UnitId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "UnitId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModelCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModelDocument", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", "Model")
                        .WithMany("Documents")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Item", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemStatusCategory", "StatusCategory")
                        .WithMany()
                        .HasForeignKey("ItemStatusCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", "EquipmentModel")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EquipmentModel");

                    b.Navigation("StatusCategory");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemDocument", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Item", null)
                        .WithMany("Documents")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.ItemNote", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Item", null)
                        .WithMany("Notes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Maintenance", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.MaintenanceCategory", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.MaintenanceFrequency", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.MaintenanceCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Model");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Unit", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Unit", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.UserAssignment", b =>
                {
                    b.HasOne("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.User", null)
                        .WithMany("Assignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.EquipmentModel", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Item", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.User", b =>
                {
                    b.Navigation("Assignments");
                });
#pragma warning restore 612, 618
        }
    }
}

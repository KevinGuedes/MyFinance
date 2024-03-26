﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFinance.Infrastructure.Persistence.Context;

#nullable disable

namespace MyFinance.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(MyFinanceDbContext))]
    partial class MyFinanceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ArchivedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReasonToArchive")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Tag")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("AccountTags");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.BusinessUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ArchivedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Income")
                        .HasColumnType("MONEY");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Outcome")
                        .HasColumnType("MONEY");

                    b.Property<string>("ReasonToArchive")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("BusinessUnits");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.Transfer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountTagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BusinessUnitId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("RelatedTo")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Value")
                        .HasColumnType("MONEY");

                    b.HasKey("Id");

                    b.HasIndex("AccountTagId");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("UserId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.BusinessUnit", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.Transfer", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.AccountTag", "AccountTag")
                        .WithMany("Transfers")
                        .HasForeignKey("AccountTagId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyFinance.Domain.Entities.BusinessUnit", "BusinessUnit")
                        .WithMany("Transfers")
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountTag");

                    b.Navigation("BusinessUnit");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.BusinessUnit", b =>
                {
                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFinance.Infra.Data.Context;

#nullable disable

namespace MyFinance.Infra.Data.Migrations
{
    [DbContext(typeof(MyFinanceDbContext))]
    partial class MyFinanceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ArchiveDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReasonToArchive")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .IsConcurrencyToken()
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

                    b.Property<DateTime?>("ArchiveDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Income")
                        .HasPrecision(17, 4)
                        .HasColumnType("REAL");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Outcome")
                        .HasPrecision(17, 4)
                        .HasColumnType("REAL");

                    b.Property<string>("ReasonToArchive")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("BusinessUnits");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.MonthlyBalance", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BusinessUnitId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Income")
                        .HasPrecision(17, 4)
                        .HasColumnType("REAL");

                    b.Property<double>("Outcome")
                        .HasPrecision(17, 4)
                        .HasColumnType("REAL");

                    b.Property<int>("ReferenceMonth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReferenceYear")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdateDate")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("UserId");

                    b.ToTable("MonthlyBalances");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.Transfer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountTagId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MonthlyBalanceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RelatedTo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasPrecision(17, 4)
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AccountTagId");

                    b.HasIndex("MonthlyBalanceId");

                    b.HasIndex("UserId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany("AccountTags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.BusinessUnit", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany("BusinessUnits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.MonthlyBalance", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.BusinessUnit", "BusinessUnit")
                        .WithMany("MonthlyBalances")
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany("MonthlyBalances")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessUnit");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.Transfer", b =>
                {
                    b.HasOne("MyFinance.Domain.Entities.AccountTag", "AccountTag")
                        .WithMany("Transfers")
                        .HasForeignKey("AccountTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinance.Domain.Entities.MonthlyBalance", "MonthlyBalance")
                        .WithMany("Transfers")
                        .HasForeignKey("MonthlyBalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinance.Domain.Entities.User", null)
                        .WithMany("Transfers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountTag");

                    b.Navigation("MonthlyBalance");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.AccountTag", b =>
                {
                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.BusinessUnit", b =>
                {
                    b.Navigation("MonthlyBalances");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.MonthlyBalance", b =>
                {
                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("MyFinance.Domain.Entities.User", b =>
                {
                    b.Navigation("AccountTags");

                    b.Navigation("BusinessUnits");

                    b.Navigation("MonthlyBalances");

                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}

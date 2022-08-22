﻿// <auto-generated />
using GShopping.EmailAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GShopping.EmailAPI.Migrations
{
    [DbContext(typeof(MySQLContext))]
    [Migration("20220822172444_AddEmailDataTableOnDB")]
    partial class AddEmailDataTableOnDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GShopping.EmailAPI.Model.EmailLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("log");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint")
                        .HasColumnName("order_id");

                    b.Property<decimal>("SendDate")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("send_date");

                    b.HasKey("Id");

                    b.ToTable("email_log");
                });
#pragma warning restore 612, 618
        }
    }
}

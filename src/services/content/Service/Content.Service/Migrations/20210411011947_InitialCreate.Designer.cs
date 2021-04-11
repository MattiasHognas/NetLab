﻿// <auto-generated />
using System;
using Content.Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Content.Service.Migrations
{
    [DbContext(typeof(ContentContext))]
    [Migration("20210411011947_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Content.Service.Models.Content", b =>
                {
                    b.Property<long>("ContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("BookId")
                        .HasColumnType("bigint");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("PageId")
                        .HasColumnType("bigint");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ContentId");

                    b.ToTable("Contents");
                });
#pragma warning restore 612, 618
        }
    }
}
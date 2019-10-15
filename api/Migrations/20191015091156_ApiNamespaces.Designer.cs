﻿// <auto-generated />
using MD.Salary.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MD.Salary.WebApi.Migrations
{
    [DbContext(typeof(EmployeeContext))]
    [Migration("20191015091156_ApiNamespaces")]
    partial class ApiNamespaces
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MD.Salary.WebApi.Core.Models.Employee", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Group");

                    b.Property<long>("HireDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("SalaryBase")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<long>("SuperiorID");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MD.Salary.WebApi.Core.Models.Token", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("UserID");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("MD.Salary.WebApi.Core.Models.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

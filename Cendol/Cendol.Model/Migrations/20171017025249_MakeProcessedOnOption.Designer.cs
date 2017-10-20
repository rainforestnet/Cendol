using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Cendol.Model;

namespace Cendol.Model.Migrations
{
    [DbContext(typeof(CendolDbContext))]
    [Migration("20171017025249_MakeProcessedOnOption")]
    partial class MakeProcessedOnOption
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cendol.Model.InputItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Item")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ProcessedOn");

                    b.Property<string>("Status")
                        .HasMaxLength(1);

                    b.HasKey("Id");

                    b.ToTable("InputItems");
                });
        }
    }
}

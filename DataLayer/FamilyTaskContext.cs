using Core.Abstractions;
using Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer
{
    public class FamilyTaskContext : DbContext
    {

        public FamilyTaskContext(DbContextOptions<FamilyTaskContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Member>(entity => {
                    entity.HasKey(k => k.Id);
                    entity.ToTable("Member");
                })
                .Entity<Task>(entity => {
                    entity.HasKey(k => k.Id);
                    entity.ToTable("Task");
                });
        }
    }
}
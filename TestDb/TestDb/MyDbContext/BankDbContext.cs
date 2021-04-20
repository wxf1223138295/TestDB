using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestDb.Model;

namespace TestDb.MyDbContext
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        public virtual DbSet<TestBank> TestBank { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestBankConfig());
            base.OnModelCreating(modelBuilder);
        }
       
    }

    public class TestBankConfig:IEntityTypeConfiguration<TestBank>
    {
        public void Configure(EntityTypeBuilder<TestBank> builder)
        {
            builder.ToTable("testbank", "mqm");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestDb.Model;

namespace TestDb.MyDbContext
{
    public class AplloDbContext: DbContext
    {
        public AplloDbContext(DbContextOptions<AplloDbContext> options) : base(options)
        {
        }
        public virtual DbSet<TestApllo> TestApllo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestAplloConfig());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TestAplloConfig : IEntityTypeConfiguration<TestApllo>
    {
        public void Configure(EntityTypeBuilder<TestApllo> builder)
        {
            builder.ToTable("testapllo", "mqm");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name");
        }
    }
}

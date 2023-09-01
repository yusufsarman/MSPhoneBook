using Microsoft.EntityFrameworkCore;
using ReportApi.Entities;
using System.Reflection.Emit;

namespace ReportApi.Infrastructure
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Report>(entity =>
            {
                entity.ToTable("Reports"); 
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.CreateTime).IsRequired();
                entity.Property(e => e.CompletionTime);                
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>();
                // Configure the relationship with ReportDetail
                entity.HasMany(e => e.ReportDetail)
                    .WithOne(e => e.Report)
                    .HasForeignKey(e => e.ReportId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ReportDetail>(entity =>
            {
                entity.ToTable("ReportDetails"); 

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.ReportId).IsRequired();
                entity.Property(e => e.Location);
                entity.Property(e => e.PhoneCount);
                entity.Property(e => e.EmailCount);

                
                entity.HasOne(e => e.Report)
                    .WithMany(e => e.ReportDetail)
                    .HasForeignKey(e => e.ReportId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

        }
    }
}

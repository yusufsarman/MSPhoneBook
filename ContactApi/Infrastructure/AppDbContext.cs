using ContactApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactApi.Infrastructure
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Surname).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Company).HasMaxLength(200);

                entity.HasMany(e => e.ContactDetails).WithOne(e => e.Contact).HasForeignKey(e => e.ContactId);
            });

            builder.Entity<ContactDetail>(entity =>
            {
                entity.ToTable("ContactDetails");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ContactDetailType).IsRequired();

                entity.HasOne(e => e.Contact).WithMany(e => e.ContactDetails).HasForeignKey(e => e.ContactId);
            });
        }
    }
}

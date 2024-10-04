using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NanhiDuniya.Services.AuthAPI.Models;

namespace NanhiDuniya.Services.AuthAPI.Data
{
    public partial class NanhiDuniyaDbContext : IdentityDbContext<ApplicationUser>
    {
        public NanhiDuniyaDbContext()
        {
        }

        public NanhiDuniyaDbContext(DbContextOptions<NanhiDuniyaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRefreshToken>(entity
     =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RefreshToken).IsUnique(); // Ensure unique refresh tokens
                entity.Property(e => e.UserId);
                entity.Property(e => e.RefreshToken).IsRequired();
                entity.Property(e => e.Expires).IsRequired();
                entity.Property(e => e.Created).IsRequired();
                entity.Property(e => e.IsRevoked).HasDefaultValue(false);
                entity.HasIndex(e => e.IsRevoked);
            });

            modelBuilder.Entity<Admin>()
            .HasIndex(a => a.UserId)
            .IsUnique();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

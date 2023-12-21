using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models.PushNotification;

public partial class SubcriptionContext : DbContext
{
    public SubcriptionContext()
    {
    }

    public SubcriptionContext(DbContextOptions<SubcriptionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pushnotification> Pushnotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Pushnotification>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PRIMARY");

            entity.ToTable("pushnotification");

            entity.Property(e => e.SubId).ValueGeneratedNever();
            entity.Property(e => e.Auth).HasMaxLength(100);
            entity.Property(e => e.EndPoint).HasMaxLength(500);
            entity.Property(e => e.P256dh)
                .HasMaxLength(200)
                .HasColumnName("P256DH");
            entity.Property(e => e.Role).HasMaxLength(45);
            entity.Property(e => e.UserId).HasMaxLength(45);
            entity.Property(e => e.IsDelete);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

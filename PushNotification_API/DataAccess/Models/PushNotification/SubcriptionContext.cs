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

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Pushnotification> Pushnotifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=7081;database=subcription;user=localhost;password=032123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("log");

            entity.HasIndex(e => e.SubId, "SubId_idx");

            entity.Property(e => e.LogId).HasColumnName("logId");
            entity.Property(e => e.ReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.ReceivedDetail).HasColumnType("mediumtext");
            entity.Property(e => e.SentDate).HasColumnType("datetime");
            entity.Property(e => e.SentDetail).HasColumnType("mediumtext");
            entity.Property(e => e.Status).HasMaxLength(45);

            entity.HasOne(d => d.Sub).WithMany(p => p.Logs)
                .HasForeignKey(d => d.SubId)
                .HasConstraintName("SubId");
        });

        modelBuilder.Entity<Pushnotification>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PRIMARY");

            entity.ToTable("pushnotification");

            entity.HasIndex(e => e.EndPoint, "EndPoint_UNIQUE").IsUnique();

            entity.Property(e => e.Auth).HasMaxLength(100);
            entity.Property(e => e.EndPoint).HasMaxLength(500);
            entity.Property(e => e.Group).HasMaxLength(45);
            entity.Property(e => e.P256dh)
                .HasMaxLength(200)
                .HasColumnName("P256DH");
            entity.Property(e => e.UserId).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

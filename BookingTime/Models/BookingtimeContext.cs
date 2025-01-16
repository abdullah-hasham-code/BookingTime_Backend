using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Models;

public partial class BookingtimeContext : DbContext
{
    public BookingtimeContext()
    {
    }

    public BookingtimeContext(DbContextOptions<BookingtimeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AS-EFT-HASHAM\\SQLEXPRESS;Database=BOOKINGTIME;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FULL_NAME");
            entity.Property(e => e.IsVerified).HasColumnName("IS_VERIFIED");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.TokenExpireTime)
                .HasColumnType("datetime")
                .HasColumnName("TOKEN_EXPIRE_TIME");
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VERIFICATION_TOKEN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

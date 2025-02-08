using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Models;

public partial class BookingtimeContext : DbContext
{
    private readonly IConfiguration _configuration;
    public BookingtimeContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    // Constructor that takes DbContextOptions
    public BookingtimeContext(DbContextOptions<BookingtimeContext> options)
        : base(options)
    {
    }


    public virtual DbSet<PropertyDetail> PropertyDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration is not provided.");
        }

        var connectionString = _configuration.GetConnectionString("BookingTimeConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'BookingTimeConnection' is missing or empty.");
        }

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyDetail>(entity =>
        {
            entity.ToTable("PROPERTY_DETAILS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amenities)
                .HasMaxLength(200)
                .HasColumnName("AMENITIES");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASE_PRICE");
            entity.Property(e => e.CancellationOption)
                .HasMaxLength(50)
                .HasColumnName("CANCELLATION_OPTION");
            entity.Property(e => e.Charges)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CHARGES");
            entity.Property(e => e.CityId).HasColumnName("CITY_ID");
            entity.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");
            entity.Property(e => e.CurrencyId).HasColumnName("CURRENCY_ID");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Latitude)
                .HasMaxLength(100)
                .HasColumnName("LATITUDE");
            entity.Property(e => e.ListName)
                .HasMaxLength(500)
                .HasColumnName("LIST_NAME");
            entity.Property(e => e.ListTypeId).HasColumnName("LIST_TYPE_ID");
            entity.Property(e => e.LongDesc).HasColumnName("LONG_DESC");
            entity.Property(e => e.Longitude)
                .HasMaxLength(100)
                .HasColumnName("LONGITUDE");
            entity.Property(e => e.PolicyDesc).HasColumnName("POLICY_DESC");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(100)
                .HasColumnName("POSTAL_CODE");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("RATING");
            entity.Property(e => e.RoomArea)
                .HasMaxLength(50)
                .HasColumnName("ROOM_AREA");
            entity.Property(e => e.ShortDesc).HasColumnName("SHORT_DESC");
            entity.Property(e => e.StateId).HasColumnName("STATE_ID");
            entity.Property(e => e.Street).HasColumnName("STREET");
            entity.Property(e => e.TotalFloor)
                .HasMaxLength(50)
                .HasColumnName("TOTAL_FLOOR");
            entity.Property(e => e.TotalRoom)
                .HasMaxLength(50)
                .HasColumnName("TOTAL_ROOM");
            entity.Property(e => e.UsageType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("USAGE_TYPE");
        });

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

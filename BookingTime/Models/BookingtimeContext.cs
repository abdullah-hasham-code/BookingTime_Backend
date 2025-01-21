using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingTime.Models
{
    public partial class BookingtimeContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // Default constructor
        public BookingtimeContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        // Constructor that takes DbContextOptions
        public BookingtimeContext(DbContextOptions<BookingtimeContext> options)
            : base(options)
        {
        }

        // Constructor that takes IConfiguration


        public virtual DbSet<User> Users { get; set; }

        // OnConfiguring method to set the connection string
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
}

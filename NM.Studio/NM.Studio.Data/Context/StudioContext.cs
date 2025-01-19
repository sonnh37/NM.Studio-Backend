using NM.Studio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Data.Context;

public partial class StudioContext : BaseDbContext
{
    public StudioContext(DbContextOptions<StudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Color> Colors { get; set; }
    public virtual DbSet<Size> Sizes { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Photo> Photos { get; set; }
    public virtual DbSet<ProductXPhoto> ProductXPhotos { get; set; }
    public virtual DbSet<AlbumXPhoto> AlbumXPhotos { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<ProductXSize> ProductXSizes { get; set; }
    public virtual DbSet<ProductXColor> ProductXColors { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config.GetConnectionString("DefaultConnection");

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");
            
            entity.Property(x => x.Status)
                .HasConversion(new EnumToStringConverter<UserStatus>());
            
            entity.Property(x => x.Gender)
                .HasConversion(new EnumToStringConverter<Gender>());
            
            entity.Property(x => x.Role)
                .HasConversion(new EnumToStringConverter<Role>());

            entity.HasMany(m => m.Bookings)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.RefreshTokens)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");
            
            entity.Property(x => x.Status)
                .HasConversion(new EnumToStringConverter<BookingStatus>());

            entity.HasOne(sc => sc.User)
                .WithMany(c => c.Bookings)
                .HasForeignKey(sc => sc.UserId);

            entity.HasOne(sc => sc.Service)
                .WithMany(c => c.Bookings)
                .HasForeignKey(sc => sc.ServiceId);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(sc => sc.User)
                .WithMany(c => c.RefreshTokens)
                .HasForeignKey(sc => sc.UserId);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(m => m.ProductXSizes)
                .WithOne(m => m.Size)
                .HasForeignKey(m => m.SizeId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(m => m.ProductXColors)
                .WithOne(m => m.Color)
                .HasForeignKey(m => m.ColorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.ToTable("SubCategory");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(x => x.Status)
                .HasConversion(new EnumToStringConverter<ProductStatus>());

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.ProductXPhotos)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.ProductXColors)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.ProductXSizes)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.SubCategory)
                .WithMany(m => m.Products)
                .HasForeignKey(m => m.SubCategoryId);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(m => m.AlbumXPhotos)
                .WithOne(m => m.Album)
                .HasForeignKey(m => m.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.ToTable("Photo");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(m => m.AlbumsXPhotos)
                .WithOne(m => m.Photo)
                .HasForeignKey(photo => photo.AlbumId);

            entity.HasMany(m => m.ProductXPhotos)
                .WithOne(m => m.Photo)
                .HasForeignKey(photo => photo.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<AlbumXPhoto>(entity =>
        {
            entity.ToTable("AlbumXPhoto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(m => m.Album)
                .WithMany(m => m.AlbumXPhotos)
                .HasForeignKey(photo => photo.AlbumId);

            entity.HasOne(m => m.Photo)
                .WithMany(m => m.AlbumsXPhotos)
                .HasForeignKey(photo => photo.PhotoId);
        });

        modelBuilder.Entity<ProductXPhoto>(entity =>
        {
            entity.ToTable("ProductXPhoto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(m => m.Product)
                .WithMany(m => m.ProductXPhotos)
                .HasForeignKey(photo => photo.ProductId);

            entity.HasOne(m => m.Photo)
                .WithMany(m => m.ProductXPhotos)
                .HasForeignKey(photo => photo.PhotoId);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.Bookings)
                .WithOne(m => m.Service)
                .HasForeignKey(m => m.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
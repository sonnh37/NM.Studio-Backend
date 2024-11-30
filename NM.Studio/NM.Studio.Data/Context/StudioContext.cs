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

    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Color> Colors { get; set; } = null!;
    public virtual DbSet<Size> Sizes { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Photo> Photos { get; set; } = null!;
    public virtual DbSet<ProductXPhoto> ProductXPhotos { get; set; } = null!;
    public virtual DbSet<AlbumXPhoto> AlbumXPhotos { get; set; } = null!;
    public virtual DbSet<Service> Services { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Blog> Blogs { get; set; } = null!;

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
        var strConn = /*config["ConnectionStrings:DB"]*/ config.GetConnectionString("DefaultConnection");

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
        });
        
        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasMany(m => m.Products)
                .WithOne(m => m.Size)
                .HasForeignKey(m => m.SizeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasMany(m => m.Products)
                .WithOne(m => m.Color)
                .HasForeignKey(m => m.ColorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

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
                .HasDefaultValueSql("NEWID()");

            entity.HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        var statusProduct = new EnumToStringConverter<ProductStatus>();

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            
            entity.Property(x => x.Status)
                .HasConversion(statusProduct);

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.ProductXPhotos)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.SubCategory)
                .WithMany(m => m.Products)
                .HasForeignKey(m => m.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

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
                .HasDefaultValueSql("NEWID()");

            entity.HasMany(m => m.AlbumsXPhotos)
                .WithOne(m => m.Photo)
                .HasForeignKey(photo => photo.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(m => m.ProductXPhotos)
                .WithOne(m => m.Photo)
                .HasForeignKey(photo => photo.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<AlbumXPhoto>(entity =>
        {
            entity.ToTable("AlbumXPhoto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasOne(m => m.Album)
                .WithMany(m => m.AlbumXPhotos)
                .HasForeignKey(photo => photo.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Photo)
                .WithMany(m => m.AlbumsXPhotos)
                .HasForeignKey(photo => photo.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductXPhoto>(entity =>
        {
            entity.ToTable("ProductXPhoto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasOne(m => m.Product)
                .WithMany(m => m.ProductXPhotos)
                .HasForeignKey(photo => photo.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Photo)
                .WithMany(m => m.ProductXPhotos)
                .HasForeignKey(photo => photo.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

       
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
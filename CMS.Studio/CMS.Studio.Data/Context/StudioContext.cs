﻿using CMS.Studio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CMS.Studio.Data.Context;

public partial class StudioContext : BaseDbContext
{
    public StudioContext(DbContextOptions<StudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Outfit> Outfits { get; set; } = null!;
    public virtual DbSet<Color> Colors { get; set; } = null!;
    public virtual DbSet<Size> Sizes { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Photo> Photos { get; set; } = null!;
    public virtual DbSet<OutfitXPhoto> OutfitXPhotos { get; set; } = null!;
    public virtual DbSet<AlbumXPhoto> AlbumXPhotos { get; set; } = null!;
    public virtual DbSet<Service> Services { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

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
        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasMany(m => m.Outfits)
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

            entity.HasMany(m => m.Outfits)
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

            entity.HasMany(m => m.Outfits)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Outfit>(entity =>
        {
            entity.ToTable("Outfit");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.OutfitXPhotos)
                .WithOne(m => m.Outfit)
                .HasForeignKey(m => m.OutfitId)
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

            entity.HasMany(m => m.OutfitXPhotos)
                .WithOne(m => m.Photo)
                .HasForeignKey(photo => photo.OutfitId)
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
        
        modelBuilder.Entity<OutfitXPhoto>(entity =>
        {
            entity.ToTable("OutfitXPhoto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            entity.HasOne(m => m.Outfit)
                .WithMany(m => m.OutfitXPhotos)
                .HasForeignKey(photo => photo.OutfitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Photo)
                .WithMany(m => m.OutfitXPhotos)
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
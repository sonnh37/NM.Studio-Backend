using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Data.Context;

public partial class StudioContext : BaseDbContext
{
    public StudioContext(DbContextOptions<StudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductMedia> ProductMedias { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
    public virtual DbSet<MediaBase> MediaBases { get; set; }
    public virtual DbSet<HomeSlide> HomeSlides { get; set; }
    public virtual DbSet<AlbumImage> AlbumImages { get; set; }
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }
    public virtual DbSet<UserSetting> UserSettings { get; set; }
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<ServiceBooking> ServiceBookings { get; set; }
    public virtual DbSet<ProductVariant> ProductSizes { get; set; }
    public virtual DbSet<UserToken> UserTokens { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<VoucherUsageHistory> VoucherUsageHistories { get; set; }

    // Auto Enum Convert Int To String
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .ToList()
            .ForEach(e =>
            {
                e.SetTableName(NamingHelper.ToSnakeCase(e.DisplayName()));

                foreach (var p in e.GetProperties())
                    p.SetColumnName(NamingHelper.ToSnakeCase(p.Name));
            });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(m => m.UserTokens)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(m => m.UserTokens)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(m => m.UserSessions)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(m => m.UserOtps)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.Avatar)
                .WithOne()
                .HasForeignKey<User>(m => m.AvatarId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<UserSession>(entity =>
        {
        });
        
        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasOne(m => m.User)
                .WithOne(m => m.UserSetting)
                .HasForeignKey<UserSetting>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasOne(m => m.Thumbnail)
                .WithOne()
                .HasForeignKey<Service>(m => m.ThumbnailId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.BackgroundCover)
                .WithOne()
                .HasForeignKey<Service>(m => m.BackgroundCoverId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(c => c.Bookings)
                .WithOne(sc => sc.Service)
                .HasForeignKey(sc => sc.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<ServiceBooking>(entity =>
        {
            entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasOne(m => m.Thumbnail)
                .WithOne()
                .HasForeignKey<Blog>(m => m.ThumbnailId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.BackgroundCover)
                .WithOne()
                .HasForeignKey<Blog>(m => m.BackgroundCoverId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.Author)
                .WithMany()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasOne(c => c.Category)
                .WithMany(sc => sc.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(m => m.Variants)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.Thumbnail)
                .WithOne()
                .HasForeignKey<Product>(m => m.ThumbnailId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.Category)
                .WithOne()
                .HasForeignKey<Product>(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.SubCategory)
                .WithOne()
                .HasForeignKey<Product>(m => m.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasMany(m => m.AlbumImages)
                .WithOne(m => m.Album)
                .HasForeignKey(m => m.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MediaBase>(entity => {  });
        
        modelBuilder.Entity<HomeSlide>(entity =>
        {
            entity.HasOne(m => m.Slide)
                .WithOne()
                .HasForeignKey<HomeSlide>(m => m.SlideId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AlbumImage>(entity =>
        {
            entity.HasOne(m => m.Image)
                .WithMany()
                .HasForeignKey(m => m.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasMany(m => m.ProductMedias)
                .WithOne(m => m.ProductVariant)
                .HasForeignKey(m => m.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductMedia>(entity =>
        {
            entity.HasOne(m => m.MediaBase)
                .WithMany()
                .HasForeignKey(m => m.MediaBaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.Items)
                .WithOne(m => m.Cart)
                .HasForeignKey(m => m.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.OrderItems)
                .WithOne(m => m.Order)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.Payments)
                .WithOne(m => m.Order)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.OrderStatusHistories)
                .WithOne(m => m.Order)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumSpend).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaximumDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaximumSpend).HasColumnType("decimal(18, 2)");

            entity.HasMany(m => m.Orders)
                .WithOne(m => m.Voucher)
                .HasForeignKey(m => m.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.VoucherUsageHistories)
                .WithOne(m => m.Voucher)
                .HasForeignKey(m => m.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VoucherUsageHistory>(entity =>
        {
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            
            entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
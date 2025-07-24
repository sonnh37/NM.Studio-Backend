using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Utilities;

public static class IncludeHelper
{
    public static IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> queryable)
        where TEntity : BaseEntity
    {
        return (queryable switch
        {
            IQueryable<Album> albums => Album(albums) as IQueryable<TEntity>,
            IQueryable<Product> products => Product(products) as IQueryable<TEntity>,
            IQueryable<Service> services => Service(services) as IQueryable<TEntity>,
            IQueryable<Category> categories => Category(categories) as IQueryable<TEntity>,
            IQueryable<MediaFile> mediaFiles => MediaFile(mediaFiles) as IQueryable<TEntity>,
            IQueryable<Color> colors => Color(colors) as IQueryable<TEntity>,
            IQueryable<Size> sizes => Size(sizes) as IQueryable<TEntity>,
            IQueryable<ProductColor> productColors => ProductColor(productColors) as IQueryable<TEntity>,
            IQueryable<ProductSize> productSizes => ProductSize(productSizes) as IQueryable<TEntity>,
            _ => queryable
        })!;
    }

    private static IQueryable<Album> Album(IQueryable<Album> queryable)
    {
        queryable = queryable
            .Include(m => m.AlbumMedias).ThenInclude(m => m.MediaFile);

        return queryable;
    }

    private static IQueryable<Product> Product(IQueryable<Product> queryable)
    {
        queryable = queryable
            // .Include(m => m.Size)
            .Include(m => m.SubCategory).ThenInclude(m => m.Category)
            // .Include(m => m.Color)
            .Include(m => m.ProductMedias).ThenInclude(m => m.MediaFile)
            .Include(m => m.ProductColors).ThenInclude(m => m.Color)
            .Include(m => m.ProductSizes).ThenInclude(m => m.Size);

        return queryable;
    }

    private static IQueryable<Service> Service(IQueryable<Service> queryable)
    {
        queryable = queryable;

        return queryable;
    }

    private static IQueryable<Category> Category(IQueryable<Category> queryable)
    {
        queryable = queryable.Include(m => m.SubCategories);

        return queryable;
    }

    private static IQueryable<MediaFile> MediaFile(IQueryable<MediaFile> queryable)
    {
        queryable = queryable
            .Include(m => m.ProductMedias).ThenInclude(m => m.Product)
            .Include(m => m.AlbumMedias).ThenInclude(m => m.Album);

        return queryable;
    }

    private static IQueryable<Size> Size(IQueryable<Size> queryable)
    {
        queryable = queryable
            .Include(m => m.ProductSizes).ThenInclude(m => m.Product);

        return queryable;
    }

    private static IQueryable<ProductSize> ProductSize(IQueryable<ProductSize> queryable)
    {
        queryable = queryable
            .Include(m => m.Product)
            .Include(m => m.Size);

        return queryable;
    }

    private static IQueryable<ProductColor> ProductColor(IQueryable<ProductColor> queryable)
    {
        queryable = queryable
            .Include(m => m.Product)
            .Include(m => m.Color);

        return queryable;
    }

    private static IQueryable<Color> Color(IQueryable<Color> queryable)
    {
        queryable = queryable
            .Include(m => m.ProductColors).ThenInclude(m => m.Product);

        return queryable;
    }
}
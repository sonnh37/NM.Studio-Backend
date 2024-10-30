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
            IQueryable<Photo> photos => Photo(photos) as IQueryable<TEntity>,
            _ => queryable
        })!;
    }

    private static IQueryable<Album> Album(IQueryable<Album> queryable)
    {
        queryable = queryable
            .Include(m => m.AlbumXPhotos).ThenInclude(m => m.Photo);

        return queryable;
    }

    private static IQueryable<Product> Product(IQueryable<Product> queryable)
    {
        queryable = queryable
            .Include(m => m.Size)
            .Include(m => m.Category).ThenInclude(m => m.SubCategories)
            .Include(m => m.SubCategory)
            .Include(m => m.Color)
            .Include(m => m.ProductXPhotos).ThenInclude(m => m.Photo);

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

    private static IQueryable<Photo> Photo(IQueryable<Photo> queryable)
    {
        queryable = queryable
            .Include(m => m.ProductXPhotos).ThenInclude(m => m.Product)
            .Include(m => m.AlbumsXPhotos).ThenInclude(m => m.Album);

        return queryable;
    }
}
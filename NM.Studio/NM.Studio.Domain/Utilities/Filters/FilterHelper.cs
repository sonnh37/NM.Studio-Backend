using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.CQRS.Queries.Blogs;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.CQRS.Queries.Colors;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.CQRS.Queries.ProductColors;
using NM.Studio.Domain.CQRS.Queries.ProductSizes;
using NM.Studio.Domain.CQRS.Queries.Services;
using NM.Studio.Domain.CQRS.Queries.Sizes;
using NM.Studio.Domain.CQRS.Queries.SubCategories;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Utilities.Filters;

public static class FilterHelper
{
    public static IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> queryable, GetQueryableQuery query)
        where TEntity : BaseEntity
    {
        return query switch
        {
            ProductGetAllQuery productQuery =>
                Product(queryable as IQueryable<Product>, productQuery) as IQueryable<TEntity>,
            BlogGetAllQuery blogQuery =>
                Blog(queryable as IQueryable<Blog>, blogQuery) as IQueryable<TEntity>,
            MediaFileGetAllQuery mediaFileQuery =>
                MediaFile(queryable as IQueryable<MediaFile>, mediaFileQuery) as IQueryable<TEntity>,
            ServiceGetAllQuery serviceQuery =>
                Service(queryable as IQueryable<Service>, serviceQuery) as IQueryable<TEntity>,
            CategoryGetAllQuery categoryGetAllQuery =>
                Category(queryable as IQueryable<Category>, categoryGetAllQuery) as IQueryable<TEntity>,
            SubCategoryGetAllQuery subCategoryGetAllQuery =>
                SubCategory((queryable as IQueryable<SubCategory>)!, subCategoryGetAllQuery) as IQueryable<TEntity>,
            AlbumGetAllQuery albumQuery =>
                Album(queryable as IQueryable<Album>, albumQuery) as IQueryable<TEntity>,
            UserGetAllQuery userQuery =>
                User(queryable as IQueryable<User>, userQuery) as IQueryable<TEntity>,
            ProductColorGetAllQuery productColorQuery =>
                ProductColor(queryable as IQueryable<ProductColor>, productColorQuery) as IQueryable<TEntity>,
            ProductSizeGetAllQuery productSizeQuery =>
                ProductSize(queryable as IQueryable<ProductSize>, productSizeQuery) as IQueryable<TEntity>,
            SizeGetAllQuery sizeQuery =>
                Size(queryable as IQueryable<Size>, sizeQuery) as IQueryable<TEntity>,
            ColorGetAllQuery colorQuery =>
                Color(queryable as IQueryable<Color>, colorQuery) as IQueryable<TEntity>,

            _ => BaseFilterHelper.Base(queryable, query)
        };
    }

    private static IQueryable<Product> Product(IQueryable<Product> queryable, ProductGetAllQuery query)
    {
        // Lọc theo Size
        if (query.Sizes.Any())
            queryable = queryable.Where(product =>
                product.ProductSizes.Any(size => query.Sizes.Contains(size.Size!.Name!)));

        // Lọc theo Color
        if (query.Colors.Any())
            queryable = queryable.Where(product =>
                product.ProductColors.Any(color => query.Colors.Contains(color.Color!.Name!)));

        if (query.SubCategoryId != null) queryable = queryable.Where(m => m.SubCategoryId == query.SubCategoryId);

        if (!string.IsNullOrEmpty(query.CategoryName))
            queryable = queryable.Where(m =>
                m.SubCategory!.Category!.Name!.ToLower().Trim() == query.CategoryName.ToLower().Trim());

        if (query.CategoryId != null) queryable = queryable.Where(m => m.SubCategory!.Category!.Id == query.Id);

        if (!string.IsNullOrEmpty(query.SubCategoryName))
            queryable = queryable.Where(m =>
                m.SubCategory!.Name!.ToLower().Trim() == query.SubCategoryName.ToLower().Trim());

        if (!string.IsNullOrEmpty(query.Name))
            queryable = queryable.Where(m => m.Name!.ToLower().Trim().Contains(query.Name.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }

    private static IQueryable<SubCategory> SubCategory(IQueryable<SubCategory> queryable, SubCategoryGetAllQuery query)
    {
        if (query.CategoryId != null)
            queryable = queryable.Where(m => m.CategoryId != query.CategoryId);

        if (query.IsNullCategoryId.HasValue && query.IsNullCategoryId.Value)
            queryable = queryable.Where(m => m.CategoryId == null);

        return queryable;
    }

    private static IQueryable<MediaFile> MediaFile(IQueryable<MediaFile> queryable, MediaFileGetAllQuery query)
    {
        if (query.IsFeatured.HasValue) queryable = queryable.Where(m => m.IsFeatured == query.IsFeatured);

        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Href))
            queryable = queryable.Where(m => m.Href!.ToLower().Trim().Contains(query.Href.ToLower().Trim()));

        if (query.AlbumId != null)
            queryable = queryable.Where(m => !m.AlbumMedias.Select(a => a.AlbumId).Contains(query.AlbumId));

        if (query.ProductId != null)
            queryable = queryable.Where(m => !m.ProductMedias.Select(a => a.ProductId).Contains(query.ProductId));


        queryable = BaseFilterHelper.Base(queryable, query);
        return queryable;
    }

    private static IQueryable<Size> Size(IQueryable<Size> queryable, SizeGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Name))
            queryable = queryable.Where(m => m.Name!.ToLower().Trim().Contains(query.Name.ToLower().Trim()));

        if (query.ProductId != null)
            queryable = queryable.Where(m => !m.ProductSizes.Select(a => a.ProductId).Contains(query.ProductId));

        queryable = BaseFilterHelper.Base(queryable, query);
        return queryable;
    }

    private static IQueryable<Color> Color(IQueryable<Color> queryable, ColorGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Name))
            queryable = queryable.Where(m => m.Name!.ToLower().Trim().Contains(query.Name.ToLower().Trim()));

        if (query.ProductId != null)
            queryable = queryable.Where(m => !m.ProductColors.Select(a => a.ProductId).Contains(query.ProductId));

        queryable = BaseFilterHelper.Base(queryable, query);
        return queryable;
    }

    private static IQueryable<Category> Category(IQueryable<Category> queryable, CategoryGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Name))
            queryable = queryable.Where(m => m.Name!.ToLower().Trim().Contains(query.Name.ToLower().Trim()));

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }


    private static IQueryable<Service> Service(IQueryable<Service> queryable, ServiceGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Name))
            queryable = queryable.Where(m => m.Name!.ToLower().Trim().Contains(query.Name.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }

    private static IQueryable<Blog> Blog(IQueryable<Blog> queryable, BlogGetAllQuery query)
    {
        if (query.IsFeatured.HasValue) queryable = queryable.Where(m => m.IsFeatured == query.IsFeatured);

        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }

    private static IQueryable<Album> Album(IQueryable<Album> queryable, AlbumGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Description))
            queryable = queryable.Where(m => m.Description!.ToLower().Contains(query.Description.ToLower()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }


    private static IQueryable<User>? User(IQueryable<User> queryable, UserGetAllQuery query)
    {
        if (!string.IsNullOrEmpty(query.Username))
            queryable = queryable.Where(e => e.Username.Contains(query.Username));

        if (!string.IsNullOrEmpty(query.FirstName))
            queryable = queryable.Where(e => e.FirstName.Contains(query.FirstName));

        if (!string.IsNullOrEmpty(query.LastName))
            queryable = queryable.Where(e => e.LastName.Contains(query.LastName));

        if (!string.IsNullOrEmpty(query.Email))
            queryable = queryable.Where(e => e.Email.Contains(query.Email));

        if (query.Dob.HasValue)
            queryable = queryable.Where(e => e.Dob == query.Dob);

        if (!string.IsNullOrEmpty(query.Address))
            queryable = queryable.Where(e => e.Address.Contains(query.Address));

        if (!string.IsNullOrEmpty(query.Role.ToString()))
            queryable = queryable.Where(e => e.Role == query.Role);

        if (!string.IsNullOrEmpty(query.Phone))
            queryable = queryable.Where(e => e.Phone.Contains(query.Phone));

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }

    private static IQueryable<ProductSize>? ProductSize(IQueryable<ProductSize> queryable,
        ProductSizeGetAllQuery query)
    {
        if (query.ProductId != null) queryable = queryable.Where(m => m.ProductId == query.ProductId);

        if (query.IsActive.HasValue) queryable = queryable.Where(m => m.IsActive == query.IsActive);

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }

    private static IQueryable<ProductColor>? ProductColor(IQueryable<ProductColor> queryable,
        ProductColorGetAllQuery query)
    {
        if (query.ProductId != null) queryable = queryable.Where(m => m.ProductId == query.ProductId);

        if (query.IsActive.HasValue) queryable = queryable.Where(m => m.IsActive == query.IsActive);

        queryable = BaseFilterHelper.Base(queryable, query);

        return queryable;
    }
}
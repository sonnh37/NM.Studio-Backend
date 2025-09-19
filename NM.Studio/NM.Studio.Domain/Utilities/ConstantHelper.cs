namespace NM.Studio.Domain.Utilities;

public class ConstantHelper
{
    #region Url api

    private const string BaseApi = "api";

    public const string Albums = $"{BaseApi}/albums";

    public const string AlbumMedias = $"{BaseApi}/albumMedias";

    public const string Categories = $"{BaseApi}/categories";

    public const string Products = $"{BaseApi}/products";

    public const string ProductMedia = $"{BaseApi}/productMedias";

    public const string MediaFiles = $"{BaseApi}/mediaFiles";

    public const string Services = $"{BaseApi}/services";

    public const string Users = $"{BaseApi}/users";

    public const string SortFieldDefault = "CreatedDate";

    #endregion


    #region Default get query

    public const int PageNumberDefault = 1;

    public const bool IsPagination = false;

    public const int PageSizeDefault = 10;

    // public const SortOrder SortOrderDefault = SortOrder.Descending;

    #endregion
}

public static class Const
{
    #region Status Codes

    public const int SUCCESS_CODE = 1;
    public const int FAIL_CODE = -1;
    public const int ERROR_EXCEPTION_CODE = -4;

    #endregion

    #region Success Messages

    public const string SUCCESS_MSG = "Success action";
    public const string SUCCESS_SAVE_MSG = "Save successful";
    public const string SUCCESS_READ_MSG = "Read successful";
    public const string SUCCESS_DELETE_MSG = "Delete successful";

    #endregion

    #region Error Messages

    public const string FAIL_SAVE_MSG = "Save failed";
    public const string FAIL_DUPLICATE_MSG = "Already exists";
    public const string NOT_FOUND_MSG = "Not found";
    public const string FAIL_DELETE_MSG = "Delete failed";

    #endregion
}
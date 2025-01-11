using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Utilities;

public class ConstantHelper
{
    #region Url api
    private const string BaseApi = "api";

    public const string Albums = $"{BaseApi}/albums";

    public const string AlbumXPhotos = $"{BaseApi}/albumXPhotos";

    public const string Categories = $"{BaseApi}/categories";

    public const string Products = $"{BaseApi}/products";

    public const string ProductXPhoto = $"{BaseApi}/productXPhotos";

    public const string Photos = $"{BaseApi}/photos";

    public const string Services = $"{BaseApi}/services";

    public const string Users = $"{BaseApi}/users";

    public const string SortFieldDefault = "CreatedDate";
    #endregion

    
    #region Default get query
    
    public const int PageNumberDefault = 1;

    public const bool IsPagination = false;

    public const int PageSizeDefault = 10;

    public const SortOrder SortOrderDefault = SortOrder.Descending;
    
    #endregion
}

public class Const
{
    #region Error Codes

    public const int ERROR_EXCEPTION_CODE = -4;


    #endregion

    #region Success Codes

    public const int SUCCESS_CODE = 1;
   
    public const string SUCCESS_SAVE_MSG = "Data has been saved successfully.";
    public const string SUCCESS_READ_MSG = "Data retrieved successfully.";
    public const string SUCCESS_READ_GOOGLE_TOKEN_MSG = "Google account has been verified.";
    public const string SUCCESS_DELETE_MSG = "Data deleted successfully.";
    public const string SUCCESS_LOGIN_MSG = "Login successful.";



    #endregion

    #region Fail code

    public const int FAIL_CODE = -1;
    public const string FAIL_SAVE_MSG = "Save fail";
    public const string FAIL_READ_MSG = "Get fail";
    public const string FAIL_DELETE_MSG = "Delete fail";

    #endregion
    
    #region Not Found Codes

    public const int NOT_FOUND_CODE = -2;
    public const string NOT_FOUND_MSG = "Not found";
    public const string NOT_FOUND_USER_LOGIN_BY_GOOGLE_MSG = "Not found user that login by google";

    #endregion
}
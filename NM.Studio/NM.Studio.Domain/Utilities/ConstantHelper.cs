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

    public static readonly DateTime ExpirationLogin = DateTime.Now.AddHours(1);
    
}

public class Const
{
    #region Error Codes

    public static int ERROR_EXCEPTION_CODE = -4;


    #endregion

    #region Success Codes

    public static int SUCCESS_CODE = 1;
    public static string SUCCESS_SAVE_MSG = "Save data success";
    public static string SUCCESS_READ_MSG = "Get data success";
    public static string SUCCESS_READ_GOOGLE_TOKEN_MSG = "Email is verified";
    public static string SUCCESS_DELETE_MSG = "Delete data success";
    public static string SUCCESS_LOGIN_MSG = "Login success";


    #endregion

    #region Fail code

    public static int FAIL_CODE = -1;
    public static string FAIL_SAVE_MSG = "Save data fail";
    public static string FAIL_READ_GOOGLE_TOKEN_MSG = "Invalid Google Token";
    public static string FAIL_READ_MSG = "Get data fail";
    public static string FAIL_DELETE_MSG = "Delete data fail";

    #endregion

    #region Warning Code

    public static int WARNING_NO_DATA_CODE = 4;
    public static string WARNING_NO_DATA_MSG = "No data";

    #endregion

    #region Not Found Codes

    public static int NOT_FOUND_CODE = -2;
    public static string NOT_FOUND_MSG = "Not found";
    public static string NOT_FOUND_USER_LOGIN_BY_GOOGLE_MSG = "Not found user that login by google";
    public static string NOT_USERNAME_MSG = "Not found username";
    public static string NOT_PASSWORD_MSG = "Not match password";

    #endregion
}
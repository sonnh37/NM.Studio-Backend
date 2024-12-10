namespace NM.Studio.Domain.Enums;

public enum Status
{
}

public enum UserStatus
{
    Active,
    Inactive,
    Suspended
}

public enum ProductStatus
{
    // Trạng thái không xác định hoặc chưa thiết lập
    Unspecified = 0,

    // Trang phục có sẵn để thuê hoặc bán
    Available = 1,

    // Trang phục đã được thuê và không còn sẵn
    Rented = 2,

    // Trang phục đang được bảo dưỡng hoặc sửa chữa
    InMaintenance = 3,

    // Trang phục đã bán hoặc không còn được sử dụng
    Discontinued = 4
}

public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled
}

public enum BookingStatus
{
    Pending,
    Completed,
    Cancelled
}

// public enum BlogStatus
// {
//     Pending,
//     Approved,
//     Rejected
// }

public enum PackageStatus
{
    Pending,
    Approved,
    Rejected
}

public enum VoucherStatus
{
    Active,
    Inactive
}
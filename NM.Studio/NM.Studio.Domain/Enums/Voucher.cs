namespace NM.Studio.Domain.Enums;

public enum VoucherType
{
    FixedAmount, // Fixed amount off (e.g., $10 off)
    Percentage, // Percentage off (e.g., 15% off)
    FreeShipping, // Free shipping
    BuyOneGetOne, // BOGO deals
    FirstOrder, // First order discount
    ReferralReward, // Referral program voucher
    Seasonal, // Seasonal promotion
    BirthdaySpecial, // Birthday special discount
    LoyaltyReward, // Loyalty program reward
    WelcomeBack // Re-engagement campaign
}

public enum VoucherStatus
{
    Active, // Voucher is active and can be used
    Inactive, // Voucher is temporarily inactive
    Expired, // Voucher has expired
    FullyRedeemed, // Maximum usage limit reached
    Cancelled, // Voucher was cancelled
    Scheduled, // Scheduled for future activation
    Draft // In draft/preview mode
}
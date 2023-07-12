namespace YerdenYuksek.Core.Domain.Customers;

public static class CustomerDefaults
{
    #region System customer roles

    public static string AdministratorsRoleName => "Administrators";

    public static string ForumModeratorsRoleName => "ForumModerators";

    public static string RegisteredRoleName => "Registered";

    public static string GuestsRoleName => "Guests";

    public static string VendorsRoleName => "Vendors";

    #endregion

    #region System customers

    public static string SearchEngineCustomerName => "SearchEngine";

    public static string BackgroundTaskCustomerName => "BackgroundTask";

    #endregion

    #region Customer attributes

    public static string DiscountCouponCodeAttribute => "DiscountCouponCode";

    public static string GiftCardCouponCodesAttribute => "GiftCardCouponCodes";

    public static string AvatarPictureIdAttribute => "AvatarPictureId";

    public static string ForumPostCountAttribute => "ForumPostCount";

    public static string SignatureAttribute => "Signature";

    public static string PasswordRecoveryTokenAttribute => "PasswordRecoveryToken";

    public static string PasswordRecoveryTokenDateGeneratedAttribute => "PasswordRecoveryTokenDateGenerated";

    public static string AccountActivationTokenAttribute => "AccountActivationToken";

    public static string EmailRevalidationTokenAttribute => "EmailRevalidationToken";

    public static string LastVisitedPageAttribute => "LastVisitedPage";

    public static string ImpersonatedCustomerIdAttribute => "ImpersonatedCustomerId";

    public static string AdminAreaStoreScopeConfigurationAttribute => "AdminAreaStoreScopeConfiguration";

    public static string SelectedPaymentMethodAttribute => "SelectedPaymentMethod";

    public static string SelectedShippingOptionAttribute => "SelectedShippingOption";

    public static string SelectedPickupPointAttribute => "SelectedPickupPoint";

    public static string CheckoutAttributes => "CheckoutAttributes";

    public static string OfferedShippingOptionsAttribute => "OfferedShippingOptions";

    public static string LastContinueShoppingPageAttribute => "LastContinueShoppingPage";

    public static string NotifiedAboutNewPrivateMessagesAttribute => "NotifiedAboutNewPrivateMessages";

    public static string WorkingThemeNameAttribute => "WorkingThemeName";

    public static string UseRewardPointsDuringCheckoutAttribute => "UseRewardPointsDuringCheckout";

    public static string EuCookieLawAcceptedAttribute => "EuCookieLaw.Accepted";

    public static string SelectedMultiFactorAuthenticationProviderAttribute => "SelectedMultiFactorAuthProvider";

    public static string CustomerMultiFactorAuthenticationInfo => "CustomerMultiFactorAuthenticationInfo";

    public static string HideConfigurationStepsAttribute => "HideConfigurationSteps";

    public static string CloseConfigurationStepsAttribute => "CloseConfigurationSteps";

    #endregion
}
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Customers;

public class CustomerSettings : ISettings
{
    public bool UsernamesEnabled { get; set; }

    public bool CheckUsernameAvailabilityEnabled { get; set; }

    public bool AllowUsersToChangeUsernames { get; set; }

    public bool UsernameValidationEnabled { get; set; }

    public bool UsernameValidationUseRegex { get; set; }

    public string UsernameValidationRule { get; set; }

    public bool PhoneNumberValidationEnabled { get; set; }

    public bool PhoneNumberValidationUseRegex { get; set; }

    public string PhoneNumberValidationRule { get; set; }

    public PasswordFormat DefaultPasswordFormat { get; set; }

    public string HashedPasswordFormat { get; set; }

    public int PasswordMinLength { get; set; }

    public int PasswordMaxLength { get; set; }

    public bool PasswordRequireLowercase { get; set; }

    public bool PasswordRequireUppercase { get; set; }

    public bool PasswordRequireNonAlphanumeric { get; set; }

    public bool PasswordRequireDigit { get; set; }

    public int UnduplicatedPasswordsNumber { get; set; }

    public int PasswordRecoveryLinkDaysValid { get; set; }

    public int PasswordLifetime { get; set; }

    public int FailedPasswordAllowedAttempts { get; set; }

    public int FailedPasswordLockoutMinutes { get; set; }

    public bool AllowCustomersToUploadAvatars { get; set; }

    public int AvatarMaximumSizeBytes { get; set; }

    public bool DefaultAvatarEnabled { get; set; }

    public bool ShowCustomersLocation { get; set; }

    public bool ShowCustomersJoinDate { get; set; }

    public bool AllowViewingProfiles { get; set; }

    public bool NotifyNewCustomerRegistration { get; set; }

    public bool HideDownloadableProductsTab { get; set; }

    public bool HideBackInStockSubscriptionsTab { get; set; }

    public bool DownloadableProductsValidateUser { get; set; }

    public bool NewsletterEnabled { get; set; }

    public bool NewsletterTickedByDefault { get; set; }

    public bool HideNewsletterBlock { get; set; }

    public bool NewsletterBlockAllowToUnsubscribe { get; set; }

    public int OnlineCustomerMinutes { get; set; }

    public bool StoreLastVisitedPage { get; set; }

    public bool StoreIpAddresses { get; set; }

    public int LastActivityMinutes { get; set; }

    public bool SuffixDeletedCustomers { get; set; }

    public bool EnteringEmailTwice { get; set; }

    public bool RequireRegistrationForDownloadableProducts { get; set; }

    public bool AllowCustomersToCheckGiftCardBalance { get; set; }

    public int DeleteGuestTaskOlderThanMinutes { get; set; }

    #region Form fields

    public bool FirstNameEnabled { get; set; }

    public bool FirstNameRequired { get; set; }

    public bool LastNameEnabled { get; set; }

    public bool LastNameRequired { get; set; }

    public bool GenderEnabled { get; set; }

    public bool DateOfBirthEnabled { get; set; }

    public bool DateOfBirthRequired { get; set; }

    public int? DateOfBirthMinimumAge { get; set; }

    public bool StreetAddressEnabled { get; set; }

    public bool StreetAddressRequired { get; set; }

    public bool StreetAddress2Enabled { get; set; }

    public bool StreetAddress2Required { get; set; }

    public bool ZipCodeEnabled { get; set; }

    public bool ZipCodeRequired { get; set; }

    public bool CityEnabled { get; set; }

    public bool CityRequired { get; set; }

    public bool CountyEnabled { get; set; }

    public bool CountyRequired { get; set; }

    public bool CountryEnabled { get; set; }

    public bool CountryRequired { get; set; }

    public int? DefaultCountryId { get; set; }

    public bool PhoneEnabled { get; set; }

    public bool PhoneRequired { get; set; }

    public bool AcceptPrivacyPolicyEnabled { get; set; }

    #endregion
}
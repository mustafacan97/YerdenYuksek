﻿namespace YerdenYuksek.Core.Domain.Messages;

public static class EmailTemplateSystemNames
{
    #region Customer

    public const string CustomerRegisteredStoreOwnerNotification = "NewCustomer.Notification";

    public const string CustomerWelcomeMessage = "Customer.WelcomeMessage";

    public const string CustomerEmailValidationMessage = "Customer.EmailValidationMessage";

    public const string CustomerEmailRevalidationMessage = "Customer.EmailRevalidationMessage";

    public const string CustomerPasswordRecoveryMessage = "Customer.PasswordRecovery";

    #endregion

    #region Order

    public const string OrderPlacedVendorNotification = "OrderPlaced.VendorNotification";

    public const string OrderPlacedStoreOwnerNotification = "OrderPlaced.StoreOwnerNotification";

    public const string OrderPlacedAffiliateNotification = "OrderPlaced.AffiliateNotification";

    public const string OrderPaidStoreOwnerNotification = "OrderPaid.StoreOwnerNotification";

    public const string OrderPaidCustomerNotification = "OrderPaid.CustomerNotification";

    public const string OrderPaidVendorNotification = "OrderPaid.VendorNotification";

    public const string OrderPaidAffiliateNotification = "OrderPaid.AffiliateNotification";

    public const string OrderPlacedCustomerNotification = "OrderPlaced.CustomerNotification";

    public const string ShipmentSentCustomerNotification = "ShipmentSent.CustomerNotification";

    public const string ShipmentReadyForPickupCustomerNotification = "ShipmentReadyForPickup.CustomerNotification";

    public const string ShipmentDeliveredCustomerNotification = "ShipmentDelivered.CustomerNotification";

    public const string OrderProcessingCustomerNotification = "OrderProcessing.CustomerNotification";

    public const string OrderCompletedCustomerNotification = "OrderCompleted.CustomerNotification";

    public const string OrderCancelledCustomerNotification = "OrderCancelled.CustomerNotification";

    public const string OrderRefundedStoreOwnerNotification = "OrderRefunded.StoreOwnerNotification";

    public const string OrderRefundedCustomerNotification = "OrderRefunded.CustomerNotification";

    public const string NewOrderNoteAddedCustomerNotification = "Customer.NewOrderNote";

    public const string RecurringPaymentCancelledStoreOwnerNotification = "RecurringPaymentCancelled.StoreOwnerNotification";

    public const string RecurringPaymentCancelledCustomerNotification = "RecurringPaymentCancelled.CustomerNotification";

    public const string RecurringPaymentFailedCustomerNotification = "RecurringPaymentFailed.CustomerNotification";

    #endregion

    #region Newsletter

    public const string NewsletterSubscriptionActivationMessage = "NewsLetterSubscription.ActivationMessage";

    public const string NewsletterSubscriptionDeactivationMessage = "NewsLetterSubscription.DeactivationMessage";

    #endregion

    #region To friend

    public const string EmailAFriendMessage = "Service.EmailAFriend";

    public const string WishlistToFriendMessage = "Wishlist.EmailAFriend";

    #endregion

    #region Return requests

    public const string NewReturnRequestStoreOwnerNotification = "NewReturnRequest.StoreOwnerNotification";

    public const string NewReturnRequestCustomerNotification = "NewReturnRequest.CustomerNotification";

    public const string ReturnRequestStatusChangedCustomerNotification = "ReturnRequestStatusChanged.CustomerNotification";

    #endregion

    #region Forum

    public const string NewForumTopicMessage = "Forums.NewForumTopic";

    public const string NewForumPostMessage = "Forums.NewForumPost";

    public const string PrivateMessageNotification = "Customer.NewPM";

    #endregion

    #region Misc

    public const string NewVendorAccountApplyStoreOwnerNotification = "VendorAccountApply.StoreOwnerNotification";

    public const string VendorInformationChangeStoreOwnerNotification = "VendorInformationChange.StoreOwnerNotification";

    public const string GiftCardNotification = "GiftCard.Notification";

    public const string ProductReviewStoreOwnerNotification = "Product.ProductReview";

    public const string ProductReviewReplyCustomerNotification = "ProductReview.Reply.CustomerNotification";

    public const string QuantityBelowStoreOwnerNotification = "QuantityBelow.StoreOwnerNotification";

    public const string QuantityBelowAttributeCombinationStoreOwnerNotification = "QuantityBelow.AttributeCombination.StoreOwnerNotification";

    public const string NewVatSubmittedStoreOwnerNotification = "NewVATSubmitted.StoreOwnerNotification";

    public const string BlogCommentStoreOwnerNotification = "Blog.BlogComment";

    public const string NewsCommentStoreOwnerNotification = "News.NewsComment";

    public const string BackInStockNotification = "Customer.BackInStock";

    public const string ContactUsMessage = "Service.ContactUs";

    public const string ContactVendorMessage = "Service.ContactVendor";

    #endregion
}
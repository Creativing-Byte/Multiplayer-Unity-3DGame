using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;
using TMPro;

    public class LocalReceiptValidation : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;
        IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

        CrossPlatformValidator m_Validator = null;

        //Your products IDs. They should match the ids of your products in your store.

        private string PunchOfGems = "coinspunch420";
        private string BucketOfGems = "gemsbucket1000";
        private string GemsChest = "gemschest1500";
        private string subscriptionProductId = "premiumcasinoacces";

        public ProductType productType = ProductType.Consumable;

        private DataManager DataManager;


        public Toggle appleCertificateToggle; //Solo para pruebas en app store

        bool m_UseAppleStoreKitTestCertificate;

        void Start()
        {
            appleCertificateToggle.onValueChanged.AddListener(OnAppleStoreKitTestCertificateChanged);
            m_UseAppleStoreKitTestCertificate = appleCertificateToggle.isOn;
            InitializePurchasing();
            UpdateUI();
        }

        static bool IsCurrentStoreSupportedByValidator()
        {
            //The CrossPlatform validator only supports the GooglePlayStore and Apple's App Stores.
            return IsGooglePlayStoreSelected() || IsAppleAppStoreSelected();
        }

        static bool IsGooglePlayStoreSelected()
        {
            var currentAppStore = StandardPurchasingModule.Instance().appStore;
            return currentAppStore == AppStore.GooglePlay;
        }

        static bool IsAppleAppStoreSelected()
        {
            var currentAppStore = StandardPurchasingModule.Instance().appStore;
            return currentAppStore == AppStore.AppleAppStore ||
                   currentAppStore == AppStore.MacAppStore;
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);

            builder.AddProduct(PunchOfGems, productType);
            builder.AddProduct(BucketOfGems, productType);
            builder.AddProduct(GemsChest, productType);
            builder.AddProduct(subscriptionProductId, ProductType.Subscription);


            UnityPurchasing.Initialize(this, builder);
        }

        void OnDeferredPurchase(Product product)
        {
            Debug.Log($"Purchase of {product.definition.id} is deferred");
            UpdateUI();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;
            m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
            InitializeValidator();
            UpdateUI();
        }

        void InitializeValidator()
        {
            if (IsCurrentStoreSupportedByValidator())
            {
#if !UNITY_EDITOR
                var appleTangleData = m_UseAppleStoreKitTestCertificate ? AppleStoreKitTestTangle.Data() : AppleTangle.Data();
                m_Validator = new CrossPlatformValidator(GooglePlayTangle.Data(), appleTangleData, Application.identifier);
#endif
            }
            else
            {
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        public void BuySubscription()
        {
            m_StoreController.InitiatePurchase(subscriptionProductId);
        }
        public void BuyPunchOfGems()
        {
            m_StoreController.InitiatePurchase(PunchOfGems);
        }

        public void BuyGemsBucket()
        {
            m_StoreController.InitiatePurchase(BucketOfGems);
        }

        public void BuyGemsChest()
        {
            m_StoreController.InitiatePurchase(GemsChest);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            //Retrieve the purchased product
            var product = args.purchasedProduct;

            var isPurchaseValid = IsPurchaseValid(product);

            if (m_GooglePlayStoreExtensions.IsPurchasedProductDeferred(product))
            {
                //The purchase is Deferred.
                //Therefore, we do not unlock the content or complete the transaction.
                //ProcessPurchase will be called again once the purchase is Purchased.
                return PurchaseProcessingResult.Pending;
            }

            if (isPurchaseValid)
            {
                //Add the purchased product to the players inventory
                UnlockContent(product);
                Debug.Log("Valid receipt, unlocking content.");
            }
            else
            {
                Debug.Log("Invalid receipt, not unlocking content.");
            }

            //We return Complete, informing Unity IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        bool IsPurchaseValid(Product product)
        {
            //If we the validator doesn't support the current store, we assume the purchase is valid
            if (IsCurrentStoreSupportedByValidator())
            {
                try
                {
                    var result = m_Validator.Validate(product.receipt);
                    //The validator returns parsed receipts.
                    LogReceipts(result);
                }
                //If the purchase is deemed invalid, the validator throws an IAPSecurityException.
                catch (IAPSecurityException reason)
                {
                    Debug.Log($"Invalid receipt: {reason}");
                    return false;
                }
            }

            return true;
        }

        void UnlockContent(Product product)
        {
            if (product.definition.id == PunchOfGems)
            {
                AddGems1();
            }
            else if (product.definition.id == BucketOfGems)
            {
                AddGems2();
            }
            else if (product.definition.id == GemsChest)
            {
                AddGems3();
            }
            else if (product.definition.id == subscriptionProductId)
            {
                BuyPremium();
            }
        }

        void AddGems1()
        {
            DataManager.BuyPunchOfGems();
            UpdateUI();
        }

        void AddGems2()
        {
            DataManager.BuyGemsBucket();
            UpdateUI();
        }

        void AddGems3()
        {
            DataManager.BuyGemsChest();
            UpdateUI();
        }

        void BuyPremium()
        {
            DataManager.BuyPremiumTicket();
            UpdateUI();
        }

        bool IsSubscribedTo(Product subscription)
        {
            // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
            if (subscription.receipt == null)
            {
                return false;
            }

            //The intro_json parameter is optional and is only used for the App Store to get introductory information.
            var subscriptionManager = new SubscriptionManager(subscription, null);

            // The SubscriptionInfo contains all of the information about the subscription.
            // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
            var info = subscriptionManager.getSubscriptionInfo();

            return info.isSubscribed() == Result.True;
        }
        void UpdateUI()
        {
            var subscriptionProduct = m_StoreController.products.WithID(subscriptionProductId);

            try
            {
                var isSubscribed = IsSubscribedTo(subscriptionProduct);
                if (isSubscribed == true)
                {
                    DataManager.Updatestatusactive();
                }
                else
                {
                    DataManager.Updatestatusinactive();
                }


            }
            catch (StoreSubscriptionInfoNotSupportedException)
            {
                var receipt = (Dictionary<string, object>)MiniJson.JsonDecode(subscriptionProduct.receipt);
                var store = receipt["Store"];
            }
        }

        bool IsPurchasedProductDeferred(string productId)
        {
            var product = m_StoreController.products.WithID(productId);
            return m_GooglePlayStoreExtensions.IsPurchasedProductDeferred(product);
        }

        static void LogReceipts(IEnumerable<IPurchaseReceipt> receipts)
        {
            Debug.Log("Receipt is valid. Contents:");
            foreach (var receipt in receipts)
            {
                LogReceipt(receipt);
            }
        }

        static void LogReceipt(IPurchaseReceipt receipt)
        {
            Debug.Log($"Product ID: {receipt.productID}\n" +
                      $"Purchase Date: {receipt.purchaseDate}\n" +
                      $"Transaction ID: {receipt.transactionID}");

            if (receipt is GooglePlayReceipt googleReceipt)
            {
                Debug.Log($"Purchase State: {googleReceipt.purchaseState}\n" +
                          $"Purchase Token: {googleReceipt.purchaseToken}");
            }

            if (receipt is AppleInAppPurchaseReceipt appleReceipt)
            {
                Debug.Log($"Original Transaction ID: {appleReceipt.originalTransactionIdentifier}\n" +
                          $"Subscription Expiration Date: {appleReceipt.subscriptionExpirationDate}\n" +
                          $"Cancellation Date: {appleReceipt.cancellationDate}\n" +
                          $"Quantity: {appleReceipt.quantity}");
            }
        }


        void OnAppleStoreKitTestCertificateChanged(bool value)
        {
            m_UseAppleStoreKitTestCertificate = value;
            InitializeValidator();
        }
    }

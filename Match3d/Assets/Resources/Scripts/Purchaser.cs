using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased: 
	// "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
	// counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
	// also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	// General product identifiers for the consumable, non-consumable, and subscription products.
	// Use these handles in the code to reference which product to purchase. Also use these values 
	// when defining the Product Identifiers on the store. Except, for illustration purposes, the 
	// kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
	// specific mapping to Unity Purchasing's AddProduct, below.
	public static string kProductIDConsumable =    "consumable";   
	public static string kProductIDNonConsumable = "nonconsumable";
	public static string kProductIDSubscription =  "subscription"; 

    [Header("Remove Ads Inapp Ids")]
    public  string removeadsID =    "removeads";
    public String RemoveAdprice;
    public Text RemoveAdTxt;

    [Space(100)]
    public Text[] SpecialOfferText;
    [Header("Special Offer Inapp Id")]
    public string specialofferID = "specialoffer";
    public int asignspecialoffercoinvalue;
    public int asignspecialofferhintvalue;
    public int asignspecialoffertimefreezevalue;
    public String specialofferpreviousprice;
    public String specialofferdiscountprice;
    [Header("Master Bundle")]

    [Space(100)]
    public Text[] MasterOfferText;
    [Header("Master Offer Inapp Id")]
    public string masterofferID = "masteroffer";
    public int asignmasteroffercoinvalue;
    public int asignmasterofferhintvalue;
    public int asignmasteroffertimefreezevalue;
    public String Masterofferprice;
    [Header("Super Bundle")]

    [Space(100)]
    public Text[] SuperOfferText;
    [Header("Super BUndle Inapp Id")]
    public string SuperofferID = "superoffer";
    public int asignSuperoffercoinvalue;
    public int asignSuperofferhintvalue;
    public int asignSuperoffertimefreezevalue;
    public String superofferprice;
    [Header("Mega Bundle")]

    [Space(100)]
    public Text[] MegaOfferText;
    [Header("Mega Bundle Inapp Id")]
    public string megaofferID = "megaoffer";
    public int asignmegaoffercoinvalue;
    public int asignmegaofferhintvalue;
    public int asignmegaoffertimefreezevalue;
    public String megaofferprice;
     
    [Space(100)]
    public Text[] CoinsTexts;
    [Header("Coins Inapps Ids")]

    [Header("Coins PAck1")]
    public string coinpack1ID = "coin1";
    public string coinpack1price = "hint10";
    public int assigncoinpack1Values;

    [Header("Coins PAck2")]
    public string coinpack2ID = "coin2";
    public string coinpack2price = "hint10";
    public int assigncoinpack2Values;

    [Header("Coins PAck3")]
    public string coinpack3ID = "coin3";
    public string coinpack3price = "hint10";
    public int assigncoinpack3Values;

    [Header("Coins PAck4")]
    public string coinpack4ID = "coin4";
    public string coinpack4price = "hint10";
    public int assigncoinpack4Values;

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

	// Google Play Store-specific product identifier subscription product.
	private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original"; 

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}

        AssignAllInApps();
    }

    void AssignAllInApps()
    {
        AssignRemoveAdsText();
        AssignSpecialOfferText();
        AssignMasterBundleText();
        AssignSuperBundleText();
        AssignMegaBundleText();
        AssignCoinsPackText();
       
    }

    // Assigning Coin Texts
    void AssignCoinsPackText()
    {
        CoinsTexts[0].text = "$" + coinpack1price;
        CoinsTexts[1].text = assigncoinpack1Values.ToString()+" Coins";
        CoinsTexts[2].text = "$" + coinpack2price;
        CoinsTexts[3].text = assigncoinpack2Values.ToString() + " Coins";
        CoinsTexts[4].text = "$" + coinpack3price;
        CoinsTexts[5].text = assigncoinpack3Values.ToString() + " Coins";
        CoinsTexts[6].text = "$"+coinpack4price;
        CoinsTexts[7].text = assigncoinpack4Values.ToString() + " Coins";
    }

    // Assigning Remove Ad  Texts
    void AssignRemoveAdsText()
    {
        RemoveAdTxt.text ="$"+ RemoveAdprice.ToString();
    }

    // Assigning Special Offer Texts
    void AssignSpecialOfferText()
    {
        SpecialOfferText[0].GetComponent<Text>().text = asignspecialoffercoinvalue + " Coins";
        SpecialOfferText[1].GetComponent<Text>().text = asignspecialofferhintvalue + " Hints";
        SpecialOfferText[2].GetComponent<Text>().text = asignspecialoffertimefreezevalue + "Time Freezes";
        SpecialOfferText[3].GetComponent<Text>().text = "$"+specialofferpreviousprice;
        SpecialOfferText[4].GetComponent<Text>().text = "$" + specialofferdiscountprice;
    }

    // Assigning Master Offer Texts
    void AssignMasterBundleText()
    {
        MasterOfferText[0].GetComponent<Text>().text = asignmasteroffercoinvalue + "Coins";
        MasterOfferText[1].GetComponent<Text>().text = "x" + asignmasterofferhintvalue;
        MasterOfferText[2].GetComponent<Text>().text = "x" + asignmasteroffertimefreezevalue;
        MasterOfferText[3].GetComponent<Text>().text = "$" + Masterofferprice;
    }

    // Assigning Super Offer Texts
    void AssignSuperBundleText()
    {
        SuperOfferText[0].GetComponent<Text>().text = asignSuperoffercoinvalue + "Coins";
        SuperOfferText[1].GetComponent<Text>().text = "x" + asignSuperofferhintvalue ;
        SuperOfferText[2].GetComponent<Text>().text = "x" + asignSuperoffertimefreezevalue;
        SuperOfferText[3].GetComponent<Text>().text = "$" + superofferprice;
    }

    // Assigning Mega Offer Texts
    void AssignMegaBundleText()
    {
        MegaOfferText[0].GetComponent<Text>().text = asignmegaoffercoinvalue+"Coins";
        MegaOfferText[1].GetComponent<Text>().text = "x" + asignmegaofferhintvalue;
        MegaOfferText[2].GetComponent<Text>().text = "x" + asignmegaoffertimefreezevalue;
        MegaOfferText[3].GetComponent<Text>().text = "$" + megaofferprice;
    }

    public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.	
        builder.AddProduct(coinpack1ID, ProductType.Consumable);
        builder.AddProduct(coinpack2ID, ProductType.Consumable);
        builder.AddProduct(coinpack3ID, ProductType.Consumable);
        builder.AddProduct(coinpack4ID, ProductType.Consumable);
        builder.AddProduct(specialofferID, ProductType.Consumable);
        builder.AddProduct(masterofferID, ProductType.Consumable);
        builder.AddProduct(SuperofferID, ProductType.Consumable);
        builder.AddProduct(megaofferID, ProductType.Consumable);
        // Continue adding the non-consumable product.
        builder.AddProduct(removeadsID, ProductType.NonConsumable);
		// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
		// if the Product ID was configured differently between Apple and Google stores. Also note that
		// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
		// must only be referenced here. 
//			builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
//				{ kProductNameAppleSubscription, AppleAppStore.Name },
//				{ kProductNameGooglePlaySubscription, GooglePlay.Name },
//			});

		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

    public void RemoveAds()
    {
	    // Buy the consumable product using its general identifier. Expect a response either 
	    // through ProcessPurchase or OnPurchaseFailed asynchronously.
	    BuyProductID(removeadsID);
    }

    public void BuyCoinsPackage(int n)
    {
	    // Buy the consumable product using its general identifier. Expect a response either 
	    // through ProcessPurchase or OnPurchaseFailed asynchronously.
	    if (n == 1)
        {
		    BuyProductID(coinpack1ID);
	    }
        else if (n == 2)
        {
		    BuyProductID(coinpack2ID);
	    }
        else if (n == 3)
        {
		    BuyProductID(coinpack3ID);
	    }
        else if (n == 4)
        {
            BuyProductID(coinpack4ID);
        }
    }
   
    public void BuySpecialOfferPackage()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.

        BuyProductID(specialofferID);
    }

    public void BuyMasterOfferPackage()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.

        BuyProductID(masterofferID);
    }

    public void BuySuperOfferPackage()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.

        BuyProductID(SuperofferID);
    }

    public void BuyMegaOfferPackage()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.

        BuyProductID(megaofferID);
    }

    public void BuySubscription()
	{
		// Buy the subscription product using its the general identifier. Expect a response either 
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		// Notice how we use the general product identifier in spite of this ID being mapped to
		// custom store-specific identifiers above.
		BuyProductID(kProductIDSubscription);
	}

	void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
	    if (String.Equals(args.purchasedProduct.definition.id, coinpack1ID, StringComparison.Ordinal))
	    {
		    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            int temp = PrefManager.GetCoinsValue();
            temp = temp + assigncoinpack1Values;
            PrefManager.SetCoinsValue(temp);
        }
	    else if (String.Equals(args.purchasedProduct.definition.id, coinpack2ID, StringComparison.Ordinal)){
		    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            int temp = PrefManager.GetCoinsValue();
            temp = temp + assigncoinpack2Values;
            PrefManager.SetCoinsValue(temp);
        }
	    else if (String.Equals(args.purchasedProduct.definition.id, coinpack3ID, StringComparison.Ordinal)){
		    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            int temp = PrefManager.GetCoinsValue();
            temp = temp + assigncoinpack3Values;
            PrefManager.SetCoinsValue(temp);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coinpack3ID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            int temp = PrefManager.GetCoinsValue();
            temp = temp + assigncoinpack4Values;
            PrefManager.SetCoinsValue(temp);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, specialofferID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            
            // Set Hint Value
            int hint = PrefManager.GetMagnetsValue();
            hint = hint + asignspecialofferhintvalue;
            PrefManager.SetMagnetsValue(hint);

            // Set TimeFreeze Value
            int timefreeze = PrefManager.GetFreezeValue();
            timefreeze = timefreeze + asignspecialoffertimefreezevalue;
            PrefManager.SetFreezeValue(timefreeze);

            // Set Coins Value
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue = coinvalue + asignspecialoffercoinvalue;
            PrefManager.SetCoinsValue(coinvalue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, specialofferID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            // Set Hint Value
            int hint = PrefManager.GetMagnetsValue();
            hint = hint + asignspecialofferhintvalue;
            PrefManager.SetMagnetsValue(hint);

            // Set TimeFreeze Value
            int timefreeze = PrefManager.GetFreezeValue();
            timefreeze = timefreeze + asignspecialoffertimefreezevalue;
            PrefManager.SetFreezeValue(timefreeze);

            // Set Coins Value
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue = coinvalue + asignspecialoffercoinvalue;
            PrefManager.SetCoinsValue(coinvalue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, masterofferID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            //Set Hint Value
            int hint = PrefManager.GetMagnetsValue();
            hint = hint + asignmasterofferhintvalue;
            PrefManager.SetMagnetsValue(hint);

            //Set TimeFreeze Value
            int timefreeze = PrefManager.GetFreezeValue();
            timefreeze = timefreeze + asignmasteroffertimefreezevalue;
            PrefManager.SetFreezeValue(timefreeze);

            //Set Coins Value
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue = coinvalue + asignspecialoffercoinvalue;
            PrefManager.SetCoinsValue(coinvalue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, SuperofferID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            // Set Hint Value
            int hint = PrefManager.GetMagnetsValue();
            hint = hint + asignSuperofferhintvalue;
            PrefManager.SetMagnetsValue(hint);

            // Set TimeFreeze Value
            int timefreeze = PrefManager.GetFreezeValue();
            timefreeze = timefreeze + asignSuperoffertimefreezevalue;
            PrefManager.SetFreezeValue(timefreeze);

            // Set Coins Value
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue = coinvalue + asignSuperoffercoinvalue;
            PrefManager.SetCoinsValue(coinvalue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, megaofferID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            // Set Hint Value
            int hint = PrefManager.GetMagnetsValue();
            hint = hint + asignmegaofferhintvalue;
            PrefManager.SetMagnetsValue(hint);

            // Set TimeFreeze Value
            int timefreeze = PrefManager.GetFreezeValue();
            timefreeze = timefreeze + asignmegaoffertimefreezevalue;
            PrefManager.SetFreezeValue(timefreeze);

            // Set Coins Value
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue = coinvalue + asignmegaoffercoinvalue;
            PrefManager.SetCoinsValue(coinvalue);
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, removeadsID, StringComparison.Ordinal))
	    {
		    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            //Utils.SaveRemoveAds();
            PlayerPrefs.SetInt("removeads", 1);
        }
	    // Or ... a subscription product has been purchased by this user.
	    else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
	    {
		    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		    // TODO: The subscription item has been successfully purchased, grant this to the player.
	    }
	    // Or ... an unknown product has been purchased by this user. Fill in additional products here....
	    else 
	    {
		    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
	    }

	    // Return a flag indicating whether this product has completely been received, or if the application needs 
	    // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
	    // saving purchased products to the cloud, and when that save is delayed. 
	    return PurchaseProcessingResult.Complete;
    }

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}

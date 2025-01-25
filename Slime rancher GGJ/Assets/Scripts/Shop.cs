using TMPro;
using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour
{
    public ShopItem[] items;
    public float priceUpdateInterval = 1500f; // 25 minutes
    public Transform shopUIParent;
    public GameObject shopItemUIPrefab;
    public TextMeshProUGUI coinText;
    public Transform itemCollider;

    private int playerCoins = 0;

    void Start()
    {
        StartCoroutine(UpdatePricesCoroutine());
        PopulateShopUI();
    }

    private IEnumerator UpdatePricesCoroutine()
    {
        while (true)
        {
            foreach (var item in items)
            {
                item.UpdatePrice();
            }
            UpdateShopUI();
            yield return new WaitForSeconds(priceUpdateInterval);
        }
    }

    private void PopulateShopUI()
    {
        foreach (var item in items)
        {
            GameObject shopItemUI = Instantiate(shopItemUIPrefab, shopUIParent);
            shopItemUI.GetComponent<ShopItemUI>().Initialize(item);
        }
    }

    private void UpdateShopUI()
    {
        foreach (Transform child in shopUIParent)
        {
            child.GetComponent<ShopItemUI>().UpdateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShopItem"))
        {
            ShopItemData itemData = other.GetComponent<ShopItemData>();
            if (itemData != null)
            {
                AddCoins(itemData.itemValue);
                Destroy(other.gameObject);
            }
        }
    }

    private void AddCoins(int amount)
    {
        playerCoins += amount;
        coinText.text = $"Coins: {playerCoins}";
    }
}

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int minPrice;
    public int maxPrice;
    [HideInInspector]
    public int currentPrice;

    public void UpdatePrice()
    {
        currentPrice = Random.Range(minPrice, maxPrice + 1);
        Debug.Log($"Updated price for {itemName}: {currentPrice}");
    }
}

public class ShopItemData : MonoBehaviour
{
    public int itemValue;
}

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;

    private ShopItem shopItem;

    public void Initialize(ShopItem item)
    {
        shopItem = item;
        UpdateUI();
    }

    public void UpdateUI()
    {
        itemNameText.text = shopItem.itemName;
        priceText.text = $"Price: {shopItem.currentPrice}";
    }
}

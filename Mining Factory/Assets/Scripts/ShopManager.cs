using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, ISelect
{
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Button shopButton;
    [SerializeField] private Transform parent;

    private List<int> itemID = new();

    private ShopUnlock shopUnlock;
    private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopUnlock = DataManager.Instance.shopUnlock;
        shopButton.onClick.AddListener(() => Open());
        itemID.Add(0);
        itemID.Add(1);
        itemID.Add(2);
        itemID.Add(3);
        itemID.Add(4);
        itemID.Add(5);
        itemID.Add(6);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetShopItem()
    {
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }

        BuildingData buildingData;
        GameObject shopItem;
        foreach (int id in itemID)
        {
            int index = id;
            buildingData = DataManager.Instance.GetBuildingData(index);

            shopItem = Instantiate(shopItemPrefab, parent);

            shopItem.transform.GetChild(0).GetComponent<Image>().sprite = buildingData.icon;
            shopItem.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            shopItem.transform.GetChild(1).GetComponent<TMP_Text>().text = buildingData.name;
            shopItem.transform.GetChild(2).GetComponent<TMP_Text>().text = buildingData.price + " 골드";

            shopItem.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => BuyBuilding(index));
        }
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            SetShopItem();
        }
        else
        {
            isOpen = false;
        }
    }

    private void BuyBuilding(int id)
    {
        
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour, ISelect
{
    private Inventory inventory;

    [SerializeField] private Button[] buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        inventory = DataManager.Instance.inventory;
        await DataManager.Instance.WaitMaterialDataLoaded();

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[index].onClick.AddListener(() => SellMaterial(index));
            MaterialData materialData = DataManager.Instance.GetMaterialData(index);
            UIManager.Instance.SetMaterialText(index, inventory.materials[index], inventory.materials[index] * materialData.price);
        }
        UIManager.Instance.SetGoldCoinText(inventory.gold);
        UIManager.Instance.SetDiamondText(inventory.diamond);
    }

    private void SellMaterial(int id)
    {
        MaterialData materialData = DataManager.Instance.GetMaterialData(id);
        int totalPrice = inventory.materials[id] * materialData.price;
        inventory.gold += totalPrice;

        inventory.materials[id] = 0;

        UIManager.Instance.SetMaterialText(id, 0, 0);
        UIManager.Instance.SetGoldCoinText(inventory.gold);
    }
    
    public void Open()
    {

    }
}

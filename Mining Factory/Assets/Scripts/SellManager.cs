using TMPro;
using UnityEngine;

public class SellManager : MonoBehaviour, ISelect
{
    private Inventory inventory;
    public void Open()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = DataManager.Instance.inventory;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

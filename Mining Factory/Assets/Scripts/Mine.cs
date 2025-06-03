using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mine : MonoBehaviour, IBuilding
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject getIcon;
    [SerializeField] private Transform iconPos;

    private int baseSlot = 2;
    private int minePower = 0;

    private Inventory inventory;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    private Coroutine mining;

    private List<GameObject> linkedObject = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSlots();
        inventory = DataManager.Instance.inventory;
    }

    public int GetID()
    {
        throw new System.NotImplementedException();
    }

    public void SetBuildMode()
    {
        throw new System.NotImplementedException();
    }

    public void SetSlots()
    {
        int slotInCount = baseSlot;

        for (int i = 0; i < slotInCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);

            float height = GetComponent<SpriteRenderer>().size.y / (slotInCount + 1) * (i + 1);
            float width = GetComponent<SpriteRenderer>().size.x / 2f + 0.3f;

            slot.transform.position += new Vector3(-width, height, 0);

            slot.GetComponent<Slot>().SetDefault(3, true);
        }
    }

    public void SetState(int id)
    {
        throw new System.NotImplementedException();
    }

    public void SetConnect(GameObject otherObject, bool input)
    {
        // 채굴력 수치에 따라 철광석 채굴 진행
        if (otherObject.GetComponent<IBuilding>().GetCanGenerate())
        {
            int index = otherObject.GetComponent<IBuilding>().GetID();

            minePower += DataManager.Instance.GetBuildingData(index).reqOut.amount;

            if (mining == null)
            {
                mining = StartCoroutine(ActivateMining());
            }
        } 
    }

    private IEnumerator ActivateMining()
    {
        while (minePower > 0)
        {
            inventory.iron += minePower;
            UIManager.Instance.SetMaterialText(6, inventory.iron, inventory.iron * 100);
            GameObject icon = Instantiate(getIcon, iconPos);
            icon.transform.GetChild(1).GetComponent<TMP_Text>().text = "+1";

            yield return waitForSeconds;
        }

        mining = null;
    }

    public void SetDisconnect(GameObject otherObject, bool input)
    {
        int index = otherObject.GetComponent<IBuilding>().GetID();

        minePower -= DataManager.Instance.GetBuildingData(index).reqOut.amount;

        if (minePower <= 0 && mining != null)
        {
            StopCoroutine(mining);
            mining = null;
        }
    }

    public bool GetCanGenerate()
    {
        return true;
    }

    public void CheckConnection(GameObject otherObject)
    {
        if (!otherObject.GetComponent<IBuilding>().GetCanGenerate())
        {
            SetDisconnect(otherObject, false);
        }
    }

    public float GetCurInput()
    {
        return 0;
    }
}

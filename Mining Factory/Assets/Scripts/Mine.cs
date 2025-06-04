using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour, IBuilding
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject getIcon;
    [SerializeField] private Transform iconPos;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private MineButton mineButton;

    private int baseSlot = 2;
    private int minePower = 0;

    private Inventory inventory;
    private Field field;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    private Coroutine mining;

    private List<GameObject> linkedObject = new();
    private Dictionary<int, float> weightPerLevel = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSlots();
        inventory = DataManager.Instance.inventory;
        field = DataManager.Instance.field;
        upgradeBtn.onClick.AddListener(() => UpgradeMine());
        SetWeight();
        mineButton.SetWeight();
    }

    private void UpgradeMine()
    {
        field.mineLevel = Mathf.Min(field.mineLevel + 1, 4);

        // 레벨에 따른 가중치 설정
        SetWeight();

        mineButton.SetWeight();
    }

    private void SetWeight()
    {
        weightPerLevel.Clear();
        switch (field.mineLevel)
        {
            case 1:
                weightPerLevel.Add(1, 100f);
                break;
            case 2:
                weightPerLevel.Add(1, 70f);
                weightPerLevel.Add(2, 30f);
                break;
            case 3:
                weightPerLevel.Add(1, 50f);
                weightPerLevel.Add(2, 35f);
                weightPerLevel.Add(3, 15f);
                break;
            case 4:
                weightPerLevel.Add(1, 30f);
                weightPerLevel.Add(2, 30f);
                weightPerLevel.Add(3, 20f);
                weightPerLevel.Add(4, 10f);
                break;
        }
    }

    private int GetMaterialInLevel()
    {
        // 가중치에 따른 등장 광물 설정
        float random = Random.Range(0, 100f);
        float weightSum = 0;
        int level = 0;
        foreach (var weight in weightPerLevel)
        {
            weightSum += weight.Value;
            if (random <= weightSum)
            {
                level = weight.Key;
                break;
            }
        }

        int[] idList = DataManager.Instance.GetMaterialsInLevel(level);
        return idList[Random.Range(0, idList.Length)];
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
            int mID = GetMaterialInLevel();
            MaterialData materialData = DataManager.Instance.GetMaterialData(mID);

            inventory.materials[mID] += minePower;
            UIManager.Instance.SetMaterialText(mID, inventory.materials[mID], inventory.materials[mID] * materialData.price);

            GameObject icon = Instantiate(getIcon, iconPos);
            icon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = materialData.icon;
            icon.transform.GetChild(1).GetComponent<TMP_Text>().text = "+" + minePower;

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private Dictionary<int, BuildingData> buildingDataDict;
    private Dictionary<int, MaterialData> materialDataDict;

    private Task _buildingLoadTask;
    private Task _materialLoadTask;

    public Inventory inventory;
    public Field field;
    public Upgrade upgrade;
    public ShopUnlock shopUnlock;

    string inventoryPath;
    string fieldPath;
    string upgradePath;
    string shopUnlockPath;

    void Awake()
    {
        Instance = this;
        _buildingLoadTask = LoadBuildingData();
        _materialLoadTask = LoadMaterialData();
        LoadMaterialData();

        Init();
    }

    void Start()
    {

    }

    private void Init()
    {
        inventoryPath = Path.Combine(Application.persistentDataPath, "inventory.json");
        fieldPath = Path.Combine(Application.persistentDataPath, "field.json");
        upgradePath = Path.Combine(Application.persistentDataPath, "upgrade.json");
        shopUnlockPath = Path.Combine(Application.persistentDataPath, "shopUnlock.json");

        if (!File.Exists(inventoryPath))
        {
            inventory = new();
            SaveInventory();
        }
        else
        {
            LoadInventory();
        }

        if (!File.Exists(fieldPath))
        {
            field = new();
            SaveField();
        }
        else
        {
            LoadField();
        }

        if (!File.Exists(upgradePath))
        {
            upgrade = new();
            SaveUpgrade();
        }
        else
        {
            LoadUpgrade();
        }

        if (!File.Exists(shopUnlockPath))
        {
            shopUnlock = new();
            SaveShopUnlock();
        }
        else
        {
            LoadShopUnlock();
        }
    }

    private void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(inventoryPath, json);
    }

    private void LoadInventory()
    {
        string json = File.ReadAllText(inventoryPath);
        inventory = JsonUtility.FromJson<Inventory>(json);
    }

    private void SaveField()
    {
        string json = JsonUtility.ToJson(field, true);
        File.WriteAllText(fieldPath, json);
    }

    private void LoadField()
    {
        string json = File.ReadAllText(fieldPath);
        field = JsonUtility.FromJson<Field>(json);
    }

    private void SaveUpgrade()
    {
        string json = JsonUtility.ToJson(upgrade, true);
        File.WriteAllText(upgradePath, json);
    }

    private void LoadUpgrade()
    {
        string json = File.ReadAllText(upgradePath);
        upgrade = JsonUtility.FromJson<Upgrade>(json);
    }

    private void SaveShopUnlock()
    {
        string json = JsonUtility.ToJson(shopUnlock, true);
        File.WriteAllText(shopUnlockPath, json);
    }

    private void LoadShopUnlock()
    {
        string json = File.ReadAllText(shopUnlockPath);
        shopUnlock = JsonUtility.FromJson<ShopUnlock>(json);
    }

    private async Task LoadBuildingData()
    {
        buildingDataDict = new Dictionary<int, BuildingData>();
        var handle = Addressables.LoadAssetsAsync<BuildingData>("Building", building =>
        {
            buildingDataDict[building.id] = building;
        });

        await handle.Task;
    }

    private async Task LoadMaterialData()
    {
        materialDataDict = new Dictionary<int, MaterialData>();
        var handle = Addressables.LoadAssetsAsync<MaterialData>("Material", material =>
        {
            materialDataDict[material.id] = material;
        });

        await handle.Task;
    }

    public Task WaitBuildindDataLoaded()
    {
        return _buildingLoadTask;
    }

    public Task WaitMaterialDataLoaded()
    {
        return _materialLoadTask;
    }

    public BuildingData GetBuildingData(int id)
    {
        return buildingDataDict[id];
    }

    public MaterialData GetMaterialData(int id)
    {
        return materialDataDict[id];
    }

    public int[] GetMaterialsInLevel(int level)
    {
        return materialDataDict
            .Where(mat => mat.Value.level == level)
            .Select(mat => mat.Key)
            .ToArray();
    }
}

[Serializable]
public class Inventory
{
    public int[] materials = new int[12];
    public int gold;
    public int diamond;
    public int gear;

    public int[] buildings;
    public int[] workers;
}

[Serializable]
public class Upgrade
{

}

[Serializable]
public class Field
{
    public int mineLevel = 1;
    public BuildingInfo[] buildingInfo;
    public SlotConnectInfo[] slotConnectInfos;
}

[Serializable]
public class BuildingInfo
{
    public int uid;
    public int buildingID;
    public Vector3 pos;
    public int[] slotUid;
}

[Serializable]
public class SlotConnectInfo
{
    public int slotIn;
    public int slotOut;
}

[Serializable]
public class ShopUnlock
{
    public List<ItemInfo> unlockList;
}

[Serializable]
public class ItemInfo
{
    public int id;
    public bool unlock;
}

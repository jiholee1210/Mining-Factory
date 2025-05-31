using System;
using UnityEngine;

public enum EnergyType
{
    Energy,
    Electricity,
    Hand,
    Fire,
    Water,
    Wind,
    Nuclear,
    MinePower
}

public enum SlotType
{
    Worker,
    Electricity_Energy,
    Energy,
    MinePower
}

[Serializable]
public class SlotInfo
{
    public SlotType type;
    public int count;
}

[Serializable]
public class ReqAmount
{
    public EnergyType type;
    public int amount;
}

[CreateAssetMenu(fileName = "BuildingData", menuName = "BuildingData", order = 0)]
public class BuildingData : ScriptableObject
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public GameObject prefab;
    public int price;

    public SlotInfo[] slotIn;
    public SlotInfo[] slotOut;

    public ReqAmount[] reqIn;
    public ReqAmount reqOut;
}

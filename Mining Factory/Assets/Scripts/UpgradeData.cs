using UnityEngine;

public enum UpgradeType
{
    None,
    MinePower,
    GeneratePower,
    WorkerPower,
}
[CreateAssetMenu(fileName = "UpgradeData", menuName = "UpgradeData", order = 0)]
public class UpgradeData : ScriptableObject
{
    public int id;
    public string name;
    public string desc;
    public UpgradeType upgradeType;
    public int price;
    public float value;
    public int nextUpgrade;
}

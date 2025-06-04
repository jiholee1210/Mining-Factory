using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "MaterialData", order = 0)]
public class MaterialData : ScriptableObject
{
    public int id;
    public Sprite icon;
    public int price;
    public int level;
}

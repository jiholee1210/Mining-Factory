using UnityEngine;

[CreateAssetMenu(fileName = "WorkerData", menuName = "WorkerData", order = 0)]
public class WorkerData : ScriptableObject
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public GameObject prefab;

    public SlotInfo[] slotOut;

    public ReqAmount reqOut;
}

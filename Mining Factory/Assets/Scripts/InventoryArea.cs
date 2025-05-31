using UnityEngine;

public class InventoryArea : MonoBehaviour
{
    public static InventoryArea Instance { get; private set; }
    [SerializeField] private RectTransform[] UIRect;

    void Awake()
    {
        Instance = this;
    }

    public bool IsPointerInsideInventory(Vector2 screenPos)
    {
        foreach (var rect in UIRect)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, null))
            {
                return true;
            }
        }
        return false;
    }
}

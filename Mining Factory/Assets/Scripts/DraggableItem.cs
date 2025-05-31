using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] private Sprite invenIcon;
    public Sprite preview;
    public int id;
    public int type;

    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = preview;
        transform.GetChild(0).GetComponent<Image>().SetNativeSize();
    }
}

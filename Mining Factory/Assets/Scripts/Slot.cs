using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform lineParent;
    [SerializeField] private int slotType;
    [SerializeField] private GameObject parentBuilding;
    private Color[] color = { Color.white, Color.yellow, Color.green, Color.gray};

    private SpriteRenderer spriteRenderer;
    
    private GameObject line;
    private LineRenderer lineRenderer;

    private GameObject otherSlot;

    private Vector3 startPos;
    private Vector3 endPos;
    
    private float size;

    private bool canConnect = false;
    private bool isConnecting = false;
    private bool BuildMode = false;
    private bool isInputSlot = false;
    private int inOrOut;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineParent = BuildingManager.Instance.lineParent;
    }

    // Update is called once per frame
    void Update()
    {
        if (BuildMode && isConnecting)
        {
            lineRenderer.SetPosition(inOrOut, transform.position);
        }
    }

    public void SetBuildMode()
    {
        BuildMode = !BuildMode;
    }

    public void SetDefault(int type, bool input)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        slotType = type;
        spriteRenderer.color = color[type];
        size = spriteRenderer.size.y;
        isInputSlot = input;
        parentBuilding = transform.parent.gameObject;
    }

    public void Connect(GameObject input, GameObject slot)
    {
        line = input;
        lineRenderer = line.GetComponent<LineRenderer>();
        otherSlot = slot;
        isConnecting = true;
        inOrOut = 1;
    }

    public void Disconnect()
    {
        isConnecting = false;
        Debug.Log("연결 해제");
        bool otherSlotState = otherSlot.GetComponent<Slot>().GetSlotState();

        transform.parent.GetComponent<IBuilding>().SetDisconnect(otherSlot.transform.parent.GetComponent<IBuilding>().GetBuildingInfo(), otherSlotState, otherSlot.transform.parent.gameObject);

        line = null;
        otherSlot = null;
    }

    public int GetSlotType() {
        return slotType;
    }

    public bool GetConnectState()
    {
        return isConnecting;
    }

    public bool GetSlotState()
    {
        return isInputSlot;
    }

    private void OnMouseDown()
    {
        if (isConnecting)
        {
            Debug.Log("연결 상태 해제");
            otherSlot.GetComponent<Slot>().Disconnect();
            Destroy(line);
            Disconnect();
        }
        line = Instantiate(linePrefab, lineParent);

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        pos.y -= size / 2;

        startPos = transform.position;
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);
        inOrOut = 0;
    }

    private void OnMouseDrag()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Slot"))
        {
            otherSlot = hit.collider.gameObject;
            if (otherSlot.GetComponent<Slot>().ConnectRequest(slotType, parentBuilding) && !otherSlot.Equals(gameObject))
            {
                endPos = hit.collider.transform.position;
                endPos.z = 0;
                lineRenderer.SetPosition(1, endPos);
                canConnect = true;
            }
            else
            {
                lineRenderer.SetPosition(1, pos);
                canConnect = false;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, pos);
            canConnect = false;
        }
    }

    private void OnMouseUp()
    {
        // 연결 슬롯의 포지션을 저장해서 출력 슬롯 입력 슬롯 연결
        if (canConnect)
        {
            lineRenderer.SetPosition(1, endPos);
            isConnecting = true;
            otherSlot.GetComponent<Slot>().Connect(line, gameObject);
            bool otherSlotState = otherSlot.GetComponent<Slot>().GetSlotState();

            transform.parent.GetComponent<IBuilding>().SetConnect(otherSlot.transform.parent.GetComponent<IBuilding>().GetBuildingInfo(), otherSlotState, otherSlot.transform.parent.gameObject);
            otherSlot.transform.parent.GetComponent<IBuilding>().SetConnect(transform.parent.GetComponent<IBuilding>().GetBuildingInfo(), isInputSlot, transform.parent.gameObject);
        }
        else
        {
            Destroy(line);
        }
    }

    public bool ConnectRequest(int type, GameObject parent)
    {;
        bool isSameType = slotType.Equals(type);
        bool isSameParent = parentBuilding.Equals(parent);
        if (isConnecting || !isSameType || isSameParent)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

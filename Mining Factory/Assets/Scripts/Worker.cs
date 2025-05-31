using UnityEngine;

public class Worker : MonoBehaviour, IBuilding
{
    [SerializeField] private GameObject slotPrefab;

    private Transform originalParent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int blockCount = 0;
    private float sizeY;
    private float sizeX;
    private bool canBuild = true;
    private int id;

    private GameObject outputObject;

    private bool isBuilding = false;
    private bool slotSetting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sizeX = spriteRenderer.size.x;
        sizeY = spriteRenderer.size.y;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.E) && canBuild)
            {
                isBuilding = false;
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                if (!slotSetting)
                {
                    SetSlots();
                }
                else
                {
                    SlotBuildMode();
                }
                BuildingManager.Instance.ExitBuildMode();
            }
        }
    }
    // 연결된 건물이 본 측정기의 기준 이상의 파워를 공급하면 작동
    // 작동 시 초당 철광석 채굴량 증가
    public void SetSlots()
    {
        slotSetting = true;
        int maxCount = 0;
        int curIndex = 0;

        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(id).slotOut)
        {
            maxCount += slotInfo.count;
        }

        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(id).slotOut)
        {
            for (int i = 0; i < maxCount; i++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);

                float height = sizeY / (maxCount + 1) * (curIndex++ + 1);
                float width = sizeX / 2f + 1f;

                slot.transform.position += new Vector3(width, height, 0);

                slot.GetComponent<Slot>().SetDefault((int)slotInfo.type, false);
            }
        }
    }

    public void SetState(int itemID)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBuildMode();
        id = itemID;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);

        SlotBuildMode();
    }

    private void SlotBuildMode()
    {
        Slot slot;
        for (int i = 0; i < transform.childCount; i++)
        {
            slot = transform.GetChild(i).GetComponent<Slot>();
            slot.SetBuildMode();
        }
    }

    public int GetID()
    {
        return id;
    }

    public float GetCurInput()
    {
        return 0;
    }

    public void SetBuildMode()
    {
        isBuilding = !isBuilding;
    }

    public void SetConnect(GameObject gameObject, bool input)
    {
        outputObject = gameObject;
        animator.SetBool("IsConnect", true);
    }

    public void SetDisconnect(GameObject gameObject, bool input)
    {
        outputObject = null;
        animator.SetBool("IsConnect", false);
    }

    public void CheckConnection(GameObject otherObject)
    {
        
    }

    private void SelectItem()
    {
        Debug.Log("아이템 선택");
        BuildingManager.Instance.SelectItem(this);
    }

    public bool GetCanGenerate()
    {
        return true;
    }

    private void OnMouseDown()
    {
        if (isBuilding)
        {
            Debug.Log("마우스 드래그 시작");
            originalParent = transform.parent;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            pos.x -= sizeX / 2;
            pos.y -= sizeY / 2;
            transform.SetParent(transform.root);
        }
        else
        {
            SelectItem();
        }
    }

    private void OnMouseDrag()
    {
        if (isBuilding)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            pos.x -= sizeX / 2;
            pos.y -= sizeY / 2;
            transform.position = pos;

            if (canBuild)
            {
                spriteRenderer.color = new Color(0f, 1f, 0f, 0.6f);
            }
            else
            {
                spriteRenderer.color = new Color(1f, 0f, 0f, 0.6f);
            }
        }
    }

    private void OnMouseUp()
    {
        if (isBuilding)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            pos.x -= sizeX / 2;
            pos.y -= sizeY / 2;

            transform.position = pos;
            transform.SetParent(originalParent);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") || collision.CompareTag("Worker") || collision.CompareTag("Slot"))
        {
            blockCount++;
            canBuild = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") || collision.CompareTag("Worker") || collision.CompareTag("Slot"))
        {
            blockCount = Mathf.Max(0, blockCount - 1);
            if (blockCount == 0)
            {
                canBuild = true;
            }
        }
        
    }
}

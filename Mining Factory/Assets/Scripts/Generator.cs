using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Generator : MonoBehaviour, IBuilding
{
    [SerializeField] private GameObject slotPrefab;

    private Transform originalParent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private List<GameObject> inputGameobject = new();
    private GameObject outputGameobject;

    private int blockCount = 0;
    private float sizeX;
    private float sizeY;
    private bool canBuild = true;

    private bool isBuilding = false;
    private bool slotSetting = false;

    private BuildingInfo buildingInfo;

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

        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).slotIn)
        {
            maxCount += slotInfo.count;
        }

        int curIndex = 0;
        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).slotIn)
        {
            for (int i = 0; i < slotInfo.count; i++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);

                float height = sizeY / (maxCount + 1) * (curIndex++ + 1);
                float width = sizeX / 2f + 0.3f;

                slot.transform.position += new Vector3(-width, height, 0);

                Debug.Log(slotInfo.type);
                slot.GetComponent<Slot>().SetDefault((int)slotInfo.type, true);
            }
        }

        maxCount = 0;
        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).slotOut)
        {
            maxCount += slotInfo.count;
        }

        curIndex = 0;
        foreach (SlotInfo slotInfo in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).slotOut)
        {
            for (int i = 0; i < maxCount; i++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);

                float height = sizeY / (maxCount + 1) * (curIndex++ + 1);
                float width = sizeX / 2f + 0.3f;

                slot.transform.position += new Vector3(width, height, 0);

                slot.GetComponent<Slot>().SetDefault((int)slotInfo.type, false);
            }
        }
    }

    public void SetState()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBuildMode();
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);

        SlotBuildMode();
    }

    public void SetBuildingInfo(BuildingInfo input)
    {
        buildingInfo = input;
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

    public string GetID()
    {
        return buildingInfo.uid;
    }

    public int GetCurInput()
    {
        return buildingInfo.inputValue;
    }

    public void SetBuildMode()
    {
        isBuilding = !isBuilding;
    }

    public void SetConnect(BuildingInfo otherBuilding, bool input, GameObject otherObject)
    {
        if (!input)
        {
            Debug.Log("연결 시도1");
            if (!inputGameobject.Contains(otherObject))
            {
                inputGameobject.Add(otherObject);
            }
            int index = 0;
            int inputWorker = 0;
            EnergyType energyType;
            foreach (ReqAmount reqAmount in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).reqIn)
            {
                Debug.Log("연결 시도2");
                index = otherBuilding.buildingID;

                energyType = DataManager.Instance.GetBuildingData(index).reqOut.type;
                if (reqAmount.type.Equals(energyType) && otherBuilding.canGenerate)
                {
                    inputWorker = DataManager.Instance.GetBuildingData(index).reqOut.amount;

                    buildingInfo.inputValue += inputWorker;
                    CheckActivate(buildingInfo.inputValue, reqAmount.amount);
                }
            }

            if (outputGameobject != null)
            {
                outputGameobject.GetComponent<IBuilding>().SetConnect(buildingInfo, false, gameObject);
            }

        }
        else
        {
            outputGameobject = otherObject;
        }
    }

    public void SetDisconnect(BuildingInfo otherBuilding, bool input, GameObject otherObject)
    {
        if (!input)
        {
            int id = otherBuilding.buildingID;
            foreach (ReqAmount reqAmount in DataManager.Instance.GetBuildingData(buildingInfo.buildingID).reqIn)
            {
                int inputWorker = DataManager.Instance.GetBuildingData(id).reqOut.amount;

                EnergyType energyType = DataManager.Instance.GetBuildingData(id).reqOut.type;
                if (reqAmount.type.Equals(energyType) && otherBuilding.canGenerate)
                {
                    Debug.Log("연결 해제" + inputWorker);
                    buildingInfo.inputValue -= inputWorker;
                    CheckActivate(buildingInfo.inputValue, reqAmount.amount);
                }
            }
            inputGameobject.Remove(otherObject);

            if (outputGameobject != null)
            {
                outputGameobject.GetComponent<IBuilding>().CheckConnection(buildingInfo);
                // 인풋 오브젝트의 cangenerate 값 검사 + 수치 검사
            }
        }
        else
        {
            outputGameobject = null;
        }
    }

    public void CheckConnection(BuildingInfo otherBuilding)
    {
        if (!otherBuilding.canGenerate)
        {
            int index = otherBuilding.buildingID;

            int inputWorker = DataManager.Instance.GetBuildingData(index).reqOut.amount;

            EnergyType energyType = DataManager.Instance.GetBuildingData(index).reqOut.type;

            foreach (ReqAmount reqAmount in DataManager.Instance.GetBuildingData(index).reqIn)
            {
                if (reqAmount.type.Equals(energyType))
                {
                    Debug.Log("같은 타입 체크 확인 / 현재 수치 : " + buildingInfo.inputValue + " 감소 수치 : " + inputWorker);
                    buildingInfo.inputValue -= inputWorker;
                    CheckActivate(buildingInfo.inputValue, reqAmount.amount);
                }
            }
        }
        if (outputGameobject != null)
        {
            outputGameobject.GetComponent<IBuilding>().CheckConnection(buildingInfo);
        }
    }

    public bool GetCanGenerate()
    {
        return buildingInfo.canGenerate;
    }

    private void CheckActivate(float input, float req)
    {
        Debug.Log("기준치" + req + " 입력" + input);
        // 기준치를 넘어섰는 지 확인 후 애니메이터 수정
        if (input >= req)
        {
            buildingInfo.canGenerate = true;
            animator.SetBool("IsConnect", true);
        }
        else
        {
            buildingInfo.canGenerate = false;
            animator.SetBool("IsConnect", false);
        }
    }

    private void SelectItem()
    {
        Debug.Log("아이템 선택");
        BuildingManager.Instance.SelectItem(buildingInfo, this);
    }

    private void OnMouseDown()
    {
        if (isBuilding)
        {
            Debug.Log("마우스 드래그 시작");
            originalParent = transform.parent;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
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

    public BuildingInfo GetBuildingInfo()
    {
        return buildingInfo;
    }
}

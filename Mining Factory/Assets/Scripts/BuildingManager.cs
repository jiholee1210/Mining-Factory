using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] public Transform lineParent;

    private IBuilding building;
    private int curItem = -1;
    private bool inBuildMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!curItem.Equals(-1))
        {
            if (Input.GetKeyDown(KeyCode.B) && !inBuildMode)
            {
                SetBuildMode();
            }

            if (Input.GetKeyDown(KeyCode.R) && inBuildMode)
            {
                CollectItem();
            }
        }
    }

    public void SelectItem(IBuilding input)
    {
        if (inBuildMode) return;
        int id = input.GetID();
        // detail UI 초기화 및 {활성화 체크
        // 이후 Json으로 데이터 관리하면서 설치된 개별 건물 및 인력 데이터에 고유 ID 값을 추가해야될듯
        // 해당 고유 ID 값이 다르면 같은 건물이라도 다른 건물로 취급
        if (curItem.Equals(id))
        {
            curItem = -1;
            building = null;
            UIManager.Instance.CloseDetail();
        }
        else
        {
            curItem = id;
            building = input;
            UIManager.Instance.OpenDetail(id, input);
        }

    }

    private void SetBuildMode()
    {
        inBuildMode = true;
        building.SetState(curItem);
        // 인벤토리, 재화 UI 비활성화 및 선택된 건물 및 인력 외 다른 오브젝트가 선택되지 않도록 설정
    }

    public void ExitBuildMode()
    {
        inBuildMode = false;
    }

    private void CollectItem()
    {
        inBuildMode = false;
        // 고유 id로 접근해서 해당 오브젝트 삭제 및 인벤토리에 추가
        // 빌드 모드 중 건물 및 인력 회수
    }
}

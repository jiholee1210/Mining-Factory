using System;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance { get; private set; }
    [SerializeField] private Transform buildingList;
    [SerializeField] private GameObject slotPrefab;

    private Field field;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        field = DataManager.Instance.field;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBuild(int itemID)
    {
        GameObject prefab = DataManager.Instance.GetBuildingData(itemID).prefab;
        Vector3 pos = Camera.main.transform.position;
        pos.z = 0;
        GameObject item = Instantiate(prefab, buildingList);
        item.transform.position = pos;
        // 건축 중 상태로 설정
        // 건축 중 상태에서 건축 가능 상태 -> e키 입력 시 해당 자리에 설치
        BuildingInfo buildingInfo = new BuildingInfo();
        buildingInfo.uid = Guid.NewGuid().ToString();
        buildingInfo.buildingID = itemID;
        buildingInfo.level = 0;
        buildingInfo.pos = pos;

        item.GetComponent<IBuilding>().SetState();
        item.GetComponent<IBuilding>().SetBuildingInfo(buildingInfo);
        field.buildingInfo.Add(buildingInfo);
    }
}

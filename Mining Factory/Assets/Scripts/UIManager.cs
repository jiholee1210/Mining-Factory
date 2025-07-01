using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Transform detail;

    [SerializeField] private Animator invenAnimator;

    [SerializeField] private TMP_Text goldCoin;
    [SerializeField] private TMP_Text diamond;
    [SerializeField] private TMP_Text gear;

    [SerializeField] private Transform[] sellItems;

    [SerializeField] private Animator shop;

    [SerializeField] private GameObject upgradeReqPrefab;
    [SerializeField] private Transform reqParent;

    private Inventory inventory;
    private Dictionary<int, string> reqType = new()
    {
        { 0, "에너지" },
        { 1, "전기" },
        { 2, "수작업" },
        { 3, "화력" },
        { 4, "수력" },
        { 5, "풍력" },
        { 6, "원자력" },
        { 7, "채굴력" }
    };

    private BuildingInfo curBuilding;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        inventory = DataManager.Instance.inventory;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            invenAnimator.SetTrigger("Open");
        }
    }

    public void OpenDetail(BuildingInfo input)
    {
        Debug.Log("detail 오픈 : ");
        curBuilding = input;
        BuildingData building = DataManager.Instance.GetBuildingData(input.buildingID);

        detail.gameObject.SetActive(true);
        Transform buildingInfo = detail.GetChild(0);
        Transform upgradeInfo = detail.GetChild(1);
        buildingInfo.GetChild(0).GetComponent<TMP_Text>().text = building.name;
        buildingInfo.GetChild(1).GetComponent<TMP_Text>().text = "";
        foreach (ReqAmount reqAmount in building.reqIn)
        {
            buildingInfo.GetChild(1).GetComponent<TMP_Text>().text += reqType[(int)reqAmount.type] + " " + input.inputValue + " / " + reqAmount.amount + "\n";
        }
        buildingInfo.GetChild(3).GetComponent<TMP_Text>().text = input.canGenerate ? building.reqOut.amount + " " + reqType[(int)building.reqOut.type] : "0 " + reqType[(int)building.reqOut.type];

        Debug.Log(input.level);
        foreach (Transform req in reqParent)
        {
            Destroy(req.gameObject);
        }

        foreach (Material material in building.reqUpgrade[input.level].materials)
        {
            GameObject upObject = Instantiate(upgradeReqPrefab, reqParent);
            upObject.transform.GetChild(0).GetComponent<Image>().sprite = DataManager.Instance.GetMaterialData(material.id).icon;
            upObject.transform.GetChild(0).GetComponent<Image>().SetNativeSize();

            int inven = inventory.materials[material.id];
            int req = material.count;
            upObject.transform.GetChild(1).GetComponent<TMP_Text>().text = inven + " / " + req;
        }
        upgradeInfo.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = building.reqUpgrade[input.level].gold.ToString();
    }

    public void RenewDetail() {
        if(curBuilding == null) return;

        BuildingData building = DataManager.Instance.GetBuildingData(curBuilding.buildingID);
        Transform buildingInfo = detail.GetChild(0);

        buildingInfo.GetChild(1).GetComponent<TMP_Text>().text = "";
        foreach (ReqAmount reqAmount in building.reqIn)
        {
            buildingInfo.GetChild(1).GetComponent<TMP_Text>().text += reqType[(int)reqAmount.type] + " " + curBuilding.inputValue + " / " + reqAmount.amount + "\n";
        }
        buildingInfo.GetChild(3).GetComponent<TMP_Text>().text = curBuilding.canGenerate ? building.reqOut.amount + " " + reqType[(int)building.reqOut.type] : "0 " + reqType[(int)building.reqOut.type];

        int count = 0;
        foreach(Material material in building.reqUpgrade[curBuilding.level].materials) {
            Transform upObject = reqParent.GetChild(count++);

            int inven = inventory.materials[material.id];
            int req = material.count;

            upObject.GetChild(1).GetComponent<TMP_Text>().text = inven + " / " + req;
        }
    }

    public void CloseDetail()
    {
        Debug.Log("detail 비활성화");
        curBuilding = null;
        detail.gameObject.SetActive(false);
    }

    public IEnumerator OpenShop()
    {
        shop.SetTrigger("Open");
        float len = shop.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(len);
    }

    public void SetMaterialText(int id, int count, int price)
    {
        sellItems[id].GetChild(1).GetComponent<TMP_Text>().text = count.ToString();
        sellItems[id].GetChild(2).GetComponent<TMP_Text>().text = price.ToString();
    }

    public void SetGoldCoinText(int goldInput)
    {
        goldCoin.text = goldInput.ToString();
    }

    public void SetDiamondText(int diamondInput)
    {
        diamond.text = diamondInput.ToString();
    }

    public void SetGearText(int gearInput)
    {
        gear.text = gearInput.ToString();
    }
}

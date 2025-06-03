using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject detail;

    [SerializeField] private Animator inventory;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text input;
    [SerializeField] private TMP_Text canGenerate;

    [SerializeField] private TMP_Text goldCoin;
    [SerializeField] private TMP_Text diamond;
    [SerializeField] private TMP_Text gear;

    [SerializeField] private Transform[] sellItems;

    [SerializeField] private Animator shop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.SetTrigger("Open");
        }
    }

    public void OpenDetail(int id, IBuilding building)
    {
        Debug.Log("detail 오픈 : ");

        detail.SetActive(true);
        name.text = DataManager.Instance.GetBuildingData(id).name;
        input.text = building.GetCurInput().ToString();
        canGenerate.text = building.GetCanGenerate().ToString();
    }

    public void CloseDetail()
    {
        Debug.Log("detail 비활성화");
        detail.SetActive(false);
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

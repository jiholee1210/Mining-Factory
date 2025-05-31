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

    [SerializeField] private TMP_Text iron;
    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text diamond;
    [SerializeField] private TMP_Text gear;

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

    public void OpenShop()
    {
        shop.SetTrigger("Open");
    }

    public void SetIronText(int ironInput)
    {
        iron.text = ironInput.ToString();
    }

    public void SetGoldText(int goldInput)
    {
        gold.text = goldInput.ToString();
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

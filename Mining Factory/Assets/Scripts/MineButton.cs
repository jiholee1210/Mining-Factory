using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineButton : MonoBehaviour
{
    [SerializeField] private SpriteMask cooldownMask;
    [SerializeField] private float cooldownTime = 2f;
    [SerializeField] private GameObject getIcon;
    [SerializeField] private Transform iconPos;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private Vector3 maskStartPos;
    private Vector3 maskEndPos;

    private int minePower;

    private Inventory inventory;
    private Field field;
    private Dictionary<int, float> weightPerLevel = new();
    void Start()
    {
        inventory = DataManager.Instance.inventory;
        field = DataManager.Instance.field;
        // 쿨타임 마스크의 시작 (완전 아래)
        maskEndPos = cooldownMask.transform.localPosition;
        maskStartPos = maskEndPos - new Vector3(1.4f, 0f, 0); // 마스크를 아래로 1만큼 내려서 시작
        cooldownMask.transform.localPosition = maskStartPos;    
    }

    void OnMouseDown()
    {
        if (isOnCooldown) return;

        // 채굴 처리
        int mID = GetMaterialInLevel();
        MaterialData materialData = DataManager.Instance.GetMaterialData(mID);
        
        inventory.materials[mID] += minePower;
        UIManager.Instance.SetMaterialText(mID, inventory.materials[mID], inventory.materials[mID] * materialData.price);
        GameObject icon = Instantiate(getIcon, iconPos);
        icon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = materialData.icon;
        icon.transform.GetChild(1).GetComponent<TMP_Text>().text = "+" + minePower;

        // 쿨타임 시작
        StartCooldown();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            float ratio = cooldownTimer / cooldownTime;
            ratio = Mathf.Clamp01(ratio);

            // 마스크를 위로 이동
            cooldownMask.transform.localPosition = Vector3.Lerp(maskStartPos, maskEndPos, ratio);

            if (cooldownTimer >= cooldownTime)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
                cooldownMask.transform.localPosition = maskStartPos;
            }
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = 0f;
    }

    public void SetPower(int power)
    {
        minePower = power;
    }

    public void SetWeight()
    {
        weightPerLevel.Clear();
        switch (field.mineLevel)
        {
            case 1:
                weightPerLevel.Add(1, 100f);
                break;
            case 2:
                weightPerLevel.Add(1, 70f);
                weightPerLevel.Add(2, 30f);
                break;
            case 3:
                weightPerLevel.Add(1, 50f);
                weightPerLevel.Add(2, 35f);
                weightPerLevel.Add(3, 15f);
                break;
            case 4:
                weightPerLevel.Add(1, 30f);
                weightPerLevel.Add(2, 30f);
                weightPerLevel.Add(3, 20f);
                weightPerLevel.Add(4, 10f);
                break;
        }
    }

    private int GetMaterialInLevel()
    {
        // 가중치에 따른 등장 광물 설정
        float random = Random.Range(0, 100f);
        Debug.Log(random);
        float weightSum = 0;
        int level = 0;
        foreach (var weight in weightPerLevel)
        {
            weightSum += weight.Value;
            if (random <= weightSum)
            {
                level = weight.Key;
                break;
            }
        }
        Debug.Log(level);
        int[] idList = DataManager.Instance.GetMaterialsInLevel(level);
        Debug.Log(idList.Length);
        return idList[Random.Range(0, idList.Length)];
    }
}

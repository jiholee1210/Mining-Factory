using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour, ISelect
{
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] private Transform upgradeParent;
    // 현재 플레이어 업그레이드 데이터 기반으로 가능한 업그레이드 항목 생성

    private Upgrade upgrade;
    public void Open()
    {
        upgrade = DataManager.Instance.upgrade;

        RenewList();
    }

    private void BuyUpgrade(UpgradeData upgradeData)
    {
        // 1. 아이디 값으로 switch문 돌려서 처리
        // 2. 각각 업그레이드 별 클래스를 만들어서 처리

        upgrade.canUpgrade.Add(upgradeData.nextUpgrade);
        upgrade.complete.Add(upgradeData.id);

        RenewList();
    }

    private void RenewList()
    {
        foreach (Transform child in upgradeParent)
        {
            Destroy(child.gameObject);
        }

        foreach (int upgradeId in upgrade.canUpgrade)
        {
            UpgradeData upgradeData = DataManager.Instance.GetUpgradeData(upgradeId);

            GameObject upgradeItem = Instantiate(upgradePrefab, upgradeParent);

            upgradeItem.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(upgradeData));
        }
    }
}

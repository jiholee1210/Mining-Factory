using UnityEngine;

public class MineButtonScript : MonoBehaviour
{
    [SerializeField] private SpriteMask cooldownMask;
    [SerializeField] private float cooldownTime = 2f;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private Vector3 maskStartPos;
    private Vector3 maskEndPos;

    private Inventory inventory;
    void Start()
    {
        // 쿨타임 마스크의 시작 (완전 아래)
        maskEndPos = cooldownMask.transform.localPosition;
        maskStartPos = maskEndPos - new Vector3(1.4f, 0f, 0); // 마스크를 아래로 1만큼 내려서 시작
        cooldownMask.transform.localPosition = maskStartPos;

        inventory = DataManager.Instance.inventory;
    }

    void OnMouseDown()
    {
        if (isOnCooldown) return;

        // 채굴 처리
        inventory.iron += 1;
        UIManager.Instance.SetIronText(inventory.iron);

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
}

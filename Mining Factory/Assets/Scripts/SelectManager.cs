using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ShopState
{
    Upgrade,
    Sell,
    Shop,
    None
}

[Serializable]
public class Select
{
    public Button button;
    public GameObject select;
    public GameObject detail;
}

public class SelectManager : MonoBehaviour
{
    [SerializeField] private Select[] selects;

    private ShopState curState;
    private Select prev;

    private bool isOpening = false;
    void Start()
    {
        foreach (Select select in selects)
        {
            select.button.onClick.AddListener(() => StartCoroutine(SelectWindow(select)));
        }
    }

    private IEnumerator SelectWindow(Select input)
    {
        if (isOpening) yield break;

        isOpening = true;
        if (prev == null)
        {
            // 바로 열기
            input.detail.SetActive(true);
            input.select.GetComponent<ISelect>().Open();
            yield return StartCoroutine(UIManager.Instance.OpenShop());
            prev = input;
        }
        else
        {
            if (prev.Equals(input))
            {
                yield return StartCoroutine(UIManager.Instance.OpenShop());
                prev.select.GetComponent<ISelect>().Open();
                prev.detail.SetActive(false);
                prev = null;
            }
            else
            {
                yield return StartCoroutine(UIManager.Instance.OpenShop());
                prev.select.GetComponent<ISelect>().Open();
                prev.detail.SetActive(false);

                input.detail.SetActive(true);
                input.select.GetComponent<ISelect>().Open();
                yield return StartCoroutine(UIManager.Instance.OpenShop());

                prev = input;
            }
        }
        isOpening = false;
    }
}

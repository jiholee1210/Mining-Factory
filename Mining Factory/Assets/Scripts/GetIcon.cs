using TMPro;
using UnityEngine;

public class GetIcon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TMP_Text count;

    private float duration = 1f;
    private float temp = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        count = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0.5f * Time.deltaTime, 0);

        if (temp < duration)
        {
            temp += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, temp / duration);

            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            color = count.color;
            color.a = alpha;
            count.color = color;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ScrollLooper : MonoBehaviour
{
    [SerializeField] private List<Transform> images;
    [SerializeField] private float scrollSpeed;
    
    private float xLimit = -36f;
    private float xIncrease = 60f;

    private Animator animator;

    // Update is called once per frame
    void Start()
    {
        foreach (Transform image in images)
        {
            foreach (Transform child in image)
            {
                animator = child.GetComponent<Animator>();

                animator.SetBool("IsConnect", true);
            }
        }
    }

    void Update()
    {
        foreach (Transform img in images) {
            img.position += Vector3.left * scrollSpeed * Time.deltaTime;

            if (img.position.x <= xLimit) {
                Debug.Log(img.position.x + " " + xLimit);
                Vector3 pos = img.position;
                pos.x += xIncrease;
                img.position = pos;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenManager : MonoBehaviour
{
    [SerializeField] private List<Button> filters;
    [SerializeField] private Transform invenList;
    [SerializeField] private GameObject itemPrefab;

    private int curFilter = -1;
    private int curItem = -1;
    private Image prevImage;

    private bool isSelected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < filters.Count; i++)
        {
            int index = i;
            filters[i].onClick.AddListener(() => ChangeInven(index));
        }
    }

    void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                BuildingPlacer.Instance.StartBuild(curItem);
                isSelected = false;
                curItem = -1;
                prevImage.color = Color.white;
                prevImage = null;
            }
        }
    }

    private void ChangeInven(int filter)
    {
        if (filter == curFilter) return;

        foreach (Transform child in invenList)
        {
            Destroy(child.gameObject);
        }

        switch (filter)
        {
            case 0:
                for (int i = 0; i < 11; i++)
                {
                    int index = i;
                    GameObject item = Instantiate(itemPrefab, invenList);

                    DraggableItem draggableItem = item.GetComponent<DraggableItem>();

                    draggableItem.preview = DataManager.Instance.GetBuildingData(index).icon;
                    draggableItem.id = DataManager.Instance.GetBuildingData(index).id;
                    draggableItem.type = filter;

                    Button button = item.GetComponent<Button>();
                    Image image = item.GetComponent<Image>();

                    button.onClick.AddListener(() => SelectItem(draggableItem.id, image));
                }
                break;
            case 1:
                for (int i = 100; i < 109; i++)
                {
                    int index = i;
                    GameObject item = Instantiate(itemPrefab, invenList);

                    DraggableItem draggableItem = item.GetComponent<DraggableItem>();

                    draggableItem.preview = DataManager.Instance.GetBuildingData(index).icon;
                    draggableItem.id = DataManager.Instance.GetBuildingData(index).id;
                    draggableItem.type = filter;

                    Button button = item.GetComponent<Button>();
                    Image image = item.GetComponent<Image>();

                    button.onClick.AddListener(() => SelectItem(draggableItem.id, image));
                }
                break;
        }

        curFilter = filter;
    }

    private void SelectItem(int id, Image image)
    {
        if (curItem.Equals(id))
        {
            isSelected = false;
            curItem = -1;
            prevImage.color = Color.white;
            Debug.Log(curItem);
            // 선택 강조 표시 취소
        }
        else
        {
            isSelected = true;
            curItem = id;
            if (prevImage != null) {
                prevImage.color = Color.white;
            }
            image.color = Color.green;
            prevImage = image;
            Debug.Log(curItem);
            // 선택 강조 표시
        }
        ShowDetail(curItem);
    }

    private void ShowDetail(int id)
    {
        if (id.Equals(-1))
        {
            // 디테일 창 닫기
        }
        else
        {
            // 디테일 창 오픈
        }
    }
}

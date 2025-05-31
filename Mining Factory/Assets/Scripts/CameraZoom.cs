using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float minSize = 5.4f;
    public float maxSize = 10.8f;

    public float minX = -19.2f;
    public float maxX = 19.2f;
    public float minY = -10.8f;
    public float maxY = 10.8f;

    private Camera camera;
    private Vector3 lastMousePos;
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        HandleDrag();
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 마우스 스크롤이 없거나, UI 위일 경우 종료
        if (Mathf.Approximately(scrollInput, 0f) || InventoryArea.Instance.IsPointerInsideInventory(Input.mousePosition))
            return;

        // 마우스의 월드 좌표 (줌 전)
        Vector3 mouseWorldPosBeforeZoom = camera.ScreenToWorldPoint(Input.mousePosition);

        // 줌 조정
        float newSize = Mathf.Clamp(camera.orthographicSize - scrollInput * zoomSpeed, minSize, maxSize);
        camera.orthographicSize = newSize;

        // 마우스의 월드 좌표 (줌 후)
        Vector3 mouseWorldPosAfterZoom = camera.ScreenToWorldPoint(Input.mousePosition);

        // 마우스를 기준으로 카메라 위치 보정
        Vector3 positionDelta = mouseWorldPosBeforeZoom - mouseWorldPosAfterZoom;
        camera.transform.position += positionDelta;

        // 카메라 위치 제한
        ClampCameraPosition();
    }

    void ClampCameraPosition()
    {
        float camHeight = camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;
        Debug.Log(camHeight + " " + camWidth);
        Vector3 pos = camera.transform.position;
        Debug.Log($"{pos.x} {pos.y}");
        pos.x = Mathf.Clamp(pos.x, minX + camWidth, maxX - camWidth);
        pos.y = Mathf.Clamp(pos.y, minY + camHeight, maxY - camHeight);

        camera.transform.position = pos;
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 curPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMousePos - curPos;

            camera.transform.position += delta;
            ClampCameraPosition();
        }
    }
}

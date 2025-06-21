using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect safeArea;
    private Vector2 minAnchor;
    private Vector2 maxAnchor;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AdjustSafeArea();
    }

    void AdjustSafeArea()
    {
        // Lấy Safe Area của màn hình
        safeArea = Screen.safeArea;

        // Tính toán anchor dựa trên kích thước màn hình
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        // Chuyển đổi sang giá trị anchor chuẩn hóa (0-1)
        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        // Áp dụng anchor cho RectTransform
        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;

        // Đảm bảo vị trí và kích thước được đặt lại
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        Debug.Log($"Safe Area: {safeArea}");
        Debug.Log($"Screen Size: {Screen.width}x{Screen.height}");
    }

    // Gọi lại khi màn hình thay đổi (tùy chọn)
    void Update()
    {
        // Kiểm tra nếu Safe Area thay đổi (ví dụ: xoay màn hình)
        if (safeArea != Screen.safeArea)
        {
            AdjustSafeArea();
        }
    }
}
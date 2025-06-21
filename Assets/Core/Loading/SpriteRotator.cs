using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // Tốc độ xoay (độ/giây)

    private void Update()
    {
        // Xoay sprite quanh trục Z với tốc độ rotationSpeed
        transform.Rotate(0f, 0f, - rotationSpeed * Time.deltaTime);
    }
}
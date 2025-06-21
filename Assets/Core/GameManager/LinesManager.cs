using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> lines;
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < lines.Count; i++)
        {
            Vector3 viewportPoint = new Vector3(0.25f*(i+1), 0, 0);
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPoint);
            lines[i].transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);
        }
    }
}

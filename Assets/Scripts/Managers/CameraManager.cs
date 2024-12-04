using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 dragOrigin;

    [Header("Settings")]
    [SerializeField] private Bounds bounds;

    public static CameraManager instance;

    private void Awake()
    {
        // Singleton Logic
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        cam = GetComponentInChildren<Camera>();
    }

    public void MoveTo(Vector3 position)
    {
        cam.transform.position = new(position.x, position.y, -10f);
    }

    public void MoveBy(Vector3 displacement)
    {
        displacement.z = 0f;
        cam.transform.position += displacement;
    }

    public void HandlePan(int mouseButton)
    {
        if (Input.GetMouseButtonDown(mouseButton))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(mouseButton))
        {
            Vector3 current = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - current;

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = bounds.min.x + camWidth;
        float maxX = bounds.max.x - camWidth;
        float minY = bounds.min.y + camHeight;
        float maxY = bounds.max.x - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bounds.size);
    }
}

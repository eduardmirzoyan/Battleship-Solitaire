using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

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
}

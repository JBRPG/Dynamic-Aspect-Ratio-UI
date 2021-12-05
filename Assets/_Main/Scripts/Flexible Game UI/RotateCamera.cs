using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private Camera targetCamera;

    public void rotateCameraButton(float degrees)
    {
        targetCamera.transform.localRotation = Quaternion.Euler(0, 0, -degrees);
    }

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

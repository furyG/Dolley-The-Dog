using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float zoomSpeed;
    public float maxCamSize;
    public float minCamSize;

    protected Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel")!= 0f)
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }
        if (cam.orthographicSize >= maxCamSize) cam.orthographicSize = maxCamSize;
        if (cam.orthographicSize <= minCamSize) cam.orthographicSize = minCamSize;
    }
}

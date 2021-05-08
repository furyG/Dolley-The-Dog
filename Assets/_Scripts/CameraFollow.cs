using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static public CameraFollow camF; 

    [Header("Set In Inspector")]
    public float easing;

    [Header("Set Dynamically")]
    public Vector3 cam;
    public Transform dogTr;

    void Awake()
    {
        camF = this;
    }

    void Start()
    {
        cam = transform.position;
    }

    public void FindDogTransform(GameObject Dog)
    {
        dogTr = Dog.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dogTr == null)
        {
            return;
        }
        else
        {
            Vector3 destination = new Vector3(dogTr.transform.position.x,
    dogTr.transform.position.y, cam.z);
            destination = Vector3.Lerp(dogTr.transform.position, cam, easing);
            transform.position = destination;
        }    
    }
}

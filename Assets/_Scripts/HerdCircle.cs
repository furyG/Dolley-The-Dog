using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdCircle : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Vector3 circleScale = Vector3.one;

    void Start()
    {
        transform.localScale = circleScale * HerdSpawner.S.spawnRadius;
    }
}

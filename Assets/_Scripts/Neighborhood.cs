using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Animals> neighbors;
    private SphereCollider coll;

    private void Awake()
    {
        neighbors = new List<Animals>();
        coll = GetComponent<SphereCollider>();
        coll.radius = HerdSpawner.S.neighborDist / 10f;
    }

    private void FixedUpdate()
    {
        if(coll.radius != HerdSpawner.S.neighborDist / 10f)
        {
            coll.radius = HerdSpawner.S.neighborDist / 10f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Animals a = other.GetComponent<Animals>();
        if (a != null)
        {
            if (neighbors.IndexOf(a) == -1)
            {
                neighbors.Add(a);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Animals a = other.GetComponent<Animals>();
        if (a != null)
        {
            if(neighbors.IndexOf(a) != -1)
            {
                neighbors.Remove(a);
            }
        }
    }
    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;

            for(int i = 0;i<neighbors.Count; i++)
            {
                avg += neighbors[i].pos;
            }
            avg /= neighbors.Count;
            return avg;
        }
    }
    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for(int i = 0; i< neighbors.Count; i++)
            {
                avg += neighbors[i].rigid.velocity;
            }
            avg /= neighbors.Count;
            return avg;
        }
    }
    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i = 0; i < neighbors.Count; i++)
            {
                    delta = neighbors[i].pos - transform.position;
                    if (delta.magnitude <= HerdSpawner.S.collDist)
                    {
                        avg += neighbors[i].pos;
                        nearCount++;
                    }
                
            }
            if (nearCount == 0) return avg;

            avg /= nearCount;
            return avg;
        }
    }
}

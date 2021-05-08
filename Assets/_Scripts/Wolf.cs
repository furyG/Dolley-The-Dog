using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float speed;

    [Header("Set Dynamically")]
    public bool bited;
    public bool shit;
    public Vector3 delta;
    public Vector3 zero = Vector3.zero;
    public GameObject dogFind;
    public GameObject animalFind;

    private void Awake()
    {
        dogFind = DolleytheDog.D.gameObject;
        delta = transform.position - dogFind.transform.position;
    }
    void FindCow()
    {
        int random = Random.Range(0, HerdSpawner.animals.Count);
        animalFind = HerdSpawner.animals[random].gameObject;
    }
    void Death()
    {
        Destroy(gameObject);
    }


    private void Update()
    {
        if (!bited)
        {
            delta = transform.position - dogFind.transform.position;
        }
        else
        {
            delta = transform.position - animalFind.transform.position;
        }
        if (shit)
        {
            delta = transform.position + new Vector3(25, 25, 0);
        }
        transform.position -= new Vector3(delta.x, delta.y, zero.z) * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case ("Dog"):
                FindCow();
                bited = true;
                return;
            case ("Dung"):
                shit = true;
                Destroy(collision.gameObject);
                Invoke("Death", 4f);
                return;
            case ("Animal"):
                if (bited)
                {
                    collision.gameObject.SetActive(false);
                    bited = false;
                }
                return;           
        }        
    }
}

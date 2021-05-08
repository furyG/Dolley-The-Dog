using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsEnemy : MonoBehaviour
{
    [Header("Set In Inspector")]
    public string animalName;
    public float speed;
    public float lifeTime;

    [Header("Set Dynamically")]
    public Vector3 delta;
    public float bornTime;
    public GameObject herdCircle;
    public GameObject ExcitePrefab;
    public GameObject spawnExcite;

    protected Rigidbody rigid;
    protected GameObject herdCenterFind;
    protected Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        herdCenterFind = GameObject.Find("HerdCenter(Clone)");
        anim = GetComponent<Animator>();
        herdCircle = herdCenterFind.transform.GetChild(0).gameObject;
    }

    void Start()
    {
        delta = transform.position - herdCenterFind.transform.position;
        bornTime = Time.time;
        
        Vector3 deltaTransform = delta;
        spawnExcite = Instantiate<GameObject>(ExcitePrefab);
        spawnExcite.transform.position = herdCircle.transform.position+
            deltaTransform.normalized*HerdSpawner.S.spawnRadius;
        spawnExcite.transform.localScale = Vector3.one * 3f;
        spawnExcite.transform.SetParent(herdCircle.transform);

    }

    void Update()
    {
        if (Dog.D.barking)
        {
            anim.CrossFade(animalName + "_Death", 0);
            transform.position += delta * Time.deltaTime*1.5f;
            Invoke("Death", 1.5f);
        }
        if(Time.time > bornTime + lifeTime)
        {
            Death();
        }
        transform.position -= delta * speed * Time.deltaTime;
    }
    void Death()
    {
        Destroy(spawnExcite);
        Destroy(gameObject);
    }
}

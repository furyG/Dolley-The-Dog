using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdCenter : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;
    public enum herdMode { idle, move}

    [Header("Set In Inspector")]
    public float speed;
    public float timeRunMin;
    public float timeRunMax;
    public float timeIdleMin;
    public float timeIdleMax;
    public float timeToAnimalRun;
    public GameObject coinPrefab;
    public Vector3 coinSpawnVec;
    public GameObject animalPrefab;
    public float animalSpawnCrdMin;
    public float animalSpawnCrdMax;

    [Header("Set Dynamically")]
    public float timeNextDecision = 0f;
    public herdMode mode = herdMode.idle;
    static public int facing = 0;
    public Vector3 animalSpawnCrdnt;

    protected static Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    protected Rigidbody rigid;
    protected SphereCollider sColl;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sColl = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        Invoke("ChooseAnimal", 5);
        Invoke("SpawnAnimalEnemy", 6 - DolleytheDog.D.levelNumber * 0.4f);
    }

    void coinSpawn()
    {
        coinSpawnVec = new Vector3(Random.Range(0, 4), Random.Range(0, 4));
        GameObject coinSpawn = Instantiate<GameObject>(coinPrefab);
        coinSpawn.transform.position = POS + coinSpawnVec;
    }
    void Update()
    {
        if(DolleytheDog.D.mode == DolleytheDog.GameMode.playing)
        {
            if (Time.time >= timeNextDecision)
            {
                DecideDirection();
            }
            if (mode == herdMode.move)
            {
                rigid.isKinematic = false;
                rigid.velocity = speed * directions[facing];
            }
            else
            {
                rigid.isKinematic = true;
            }
            POS = transform.position;
        }
    }
    void DecideDirection()
    {
        switch (mode)
        {
            case herdMode.idle:
                facing = Random.Range(0, 4);
                timeNextDecision = Time.time + Random.Range(timeRunMin, timeRunMax);
                mode = herdMode.move;
                //Debug.Log("move");
                break;
            case herdMode.move:
                mode = herdMode.idle;
                timeNextDecision = Time.time + Random.Range(timeIdleMin, timeIdleMax);
                //Debug.Log("idle");
                break;
        }
    }
    void ChooseAnimal()
    {
        float chanceOfBeast = Random.Range(0f, 1f);
        Debug.Log("ChanceOfBeast: " + chanceOfBeast);
        if(chanceOfBeast >= 1 - DolleytheDog.D.levelNumber * 0.09f)
        {
            int randA = Random.Range(1, gameObject.transform.childCount);
            Animals a = transform.GetChild(randA).gameObject.GetComponent<Animals>();
            a.chosen = true;
            Invoke("ChooseAnimal", 5);
        }
        else
        {
            Invoke("ChooseAnimal", 5);
        }
    }

    void SpawnAnimalEnemy()
    {
        ChooseSpawnCoordinates();
        GameObject animalSpawn = Instantiate<GameObject>(animalPrefab);
        animalSpawn.transform.position = gameObject.transform.position + animalSpawnCrdnt;
        Invoke("SpawnAnimalEnemy", 6 - DolleytheDog.D.levelNumber * 0.4f);
    }

    void ChooseSpawnCoordinates()
    {
        float crdsX = Random.Range(animalSpawnCrdMin, animalSpawnCrdMax);
        float crdsY = Random.Range(animalSpawnCrdMin, animalSpawnCrdMax);
        animalSpawnCrdnt = new Vector3(crdsX, crdsY, 0f);
    }
}

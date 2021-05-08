using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdSpawner : MonoBehaviour
{
    static public HerdSpawner S;
    static public List<Animals> animals;

    [Header("Set In Inspector: Spawning")]
    public GameObject animalPrefab;
    public Transform herdAnchor;
    //public int numAnimals = 10;
    public float spawnRadius;
    public float spawnDelay = 0.2f;

    [Header("Set In Inspector: Animals")]
    public float velocity = 2f;
    public float neighborDist = 30f;
    public float collDist = 4f;
    public float velMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f;
    public float attractPush = 2f;
    public float attractPushDist = 5f;

    private void Awake()
    {
        S = this;
        animals = new List<Animals>();
    }
    public void InstantiateAnimals(int numOfAnimals)
    {
        for(int i = 0; i < numOfAnimals; i++)
        {
            GameObject go = Instantiate(animalPrefab);
            Animals a = go.GetComponent<Animals>();
            Debug.Log("korova poyavilas'");
            a.transform.SetParent(herdAnchor);
            animals.Add(a);
        }
        //ne rabotaet hernya
        //GameObject go = Instantiate(animalPrefab);
        //Animals a = go.GetComponent<Animals>();
        //herdAnchor = GameObject.Find("HerdCenter(Clone)").GetComponent<Transform>();
        //a.transform.SetParent(herdAnchor);
        //animals.Add(a);
        //if (animals.Count < numOfAnimals)
        //{
        //    Invoke("InstantiateAnimals", spawnDelay);
        //}
    }
}

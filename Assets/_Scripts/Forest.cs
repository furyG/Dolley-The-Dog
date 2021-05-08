using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    [Header("Set In Inspector")]
    public List<GameObject> trees = new List<GameObject>();
    public GameObject wolfPrefab;
    public float spawnRadius;
    public float spawnChance;
    public float treeNum;

    void Awake()
    {
        SpawnForest();
        Invoke("SpawnWolf", 3f);
    }
    void SpawnForest()
    {
        for(int i = 0; i < treeNum; i++)
        {
            float chance = Random.Range(0f, 1f);
            if(chance > spawnChance)
            {
                int choosePrefab = Random.Range(0, 3);
                GameObject go = Instantiate<GameObject>(trees[choosePrefab]);
                go.transform.position = Random.insideUnitSphere * spawnRadius;
                go.transform.SetParent(gameObject.transform);
                go.transform.localScale = Vector3.one * chance;
            }
        }
    }
    void SpawnWolf()
    {
        GameObject wolfSpawn = Instantiate<GameObject>(wolfPrefab);
        wolfSpawn.transform.position = gameObject.transform.position;
        wolfSpawn.transform.SetParent(gameObject.transform);
        Invoke("SpawnWolf", 8f);
    }
}

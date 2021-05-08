using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DolleytheDog : MonoBehaviour
{
    static public DolleytheDog D;
    public enum GameMode { idle, playing};

    [Header("Set in Inspector")]
    public GameObject ForestPrefab;
    public GameObject tilePrefab;
    public GameObject HerdCenterPrefab;
    public GameObject DogPrefab;
    public float LevelTimer;
    public float forestSpawnChance;
    public GameMode mode;

    [Header("Set Dynamically")]
    public int i = 1;
    public int aDead;
    public int aAlive;
    public int aNecessary;
    public int aNumber;
    public int levelNumber;
    public float StartTimer;
    public bool EachLevelChecking;
    public GameObject tileAnchorSpawn;
    public GameObject HerdCenterSpawn;
    public GameObject forestSpawn;
    public GameObject DogSpawn;
    public Transform TILEANCHOR;
    public Image TimeImage;
    public GameObject MenuFind;
    public GameObject EscPanelFind;
    
    
    public Vector2[,] tileSpawnCords;

    private void Awake()
    {
        D = this;
        mode = GameMode.idle;
        TimeImage = GameObject.Find("TimeImage").GetComponent<Image>();
        MenuFind = GameObject.Find("Menu");
        EscPanelFind = GameObject.Find("GameEscPanel");

        tileSpawnCords = new Vector2[200, 200];
        tileAnchorSpawn = new GameObject("TILEANCHOR");
        TILEANCHOR = tileAnchorSpawn.transform;

        SpawnCoordinates();
    }

    void Start()
    {
        //Invoke("SpawnAnimalEnemy", 6 - levelNumber*0.4f);
    }

    public void StartLevel()
    {
        levelNumber = i;
        EachLevelChecking = false;
        aAlive = 0;
        aDead = 0;

        Destroy(forestSpawn);
        Destroy(HerdCenterSpawn);
        HerdCenterSpawn = Instantiate<GameObject>(HerdCenterPrefab);
        HerdCenterSpawn.transform.position = Vector3.zero;
        HerdSpawner.animals.Clear();
        HerdSpawner.S.herdAnchor = HerdCenterSpawn.transform;
        Destroy(DogSpawn);
        DogSpawn = Instantiate<GameObject>(DogPrefab);
        DogSpawn.transform.position = HerdCenterSpawn.transform.position + Vector3.up * 5;

        CameraFollow.camF.FindDogTransform(DogSpawn);
        aNumber = levelNumber * 3 - 2;
        HerdSpawner.S.spawnRadius = levelNumber * 2.1f;
        HerdSpawner.S.InstantiateAnimals(aNumber);
        LevelTimer = levelNumber * 7+3f;
        StartTimer = Time.time;
        float fChance = Random.Range(0.6f, 1f+levelNumber*0.05f);
        if (fChance >= forestSpawnChance)
        {
            forestSpawn = Instantiate<GameObject>(ForestPrefab);
            forestSpawn.transform.position = new Vector3(Random.Range(15,20),Random.Range(15,20),0);
        }

        mode = GameMode.playing;
    }

    void Update()
    {
        if(mode == GameMode.playing)
        {
            TimeImage.fillAmount = (Time.time - StartTimer) / LevelTimer;
        }
        else
        {
            TimeImage.fillAmount = 0f;
        }
        if((Time.time - StartTimer) >= LevelTimer && !EachLevelChecking && mode==GameMode.playing)
        {
            CheckLevel();
        }
    }

    void CheckLevel()
    {
        EachLevelChecking = true;
        mode = GameMode.idle;
        foreach (Animals a in HerdSpawner.animals)
        {
            if (a.gameObject.activeSelf)
            {
                aAlive++;
            }
            else
            {
                aDead++;
            }
        }
        aNecessary = (aNumber / 2) + (aNumber / 4);
        if (aAlive >= aNecessary)
        {
            i++;
            Invoke("StartLevel", 5f);
        }
        else
        {
            i = 1;
            Invoke("StartLevel", 5f);
        }
    }

    void SpawnCoordinates()
    {
        Vector2 position;
        for(int i = 0; i < 200; i++)
        {
            for(int j = 0; j < 200; j++)
            {
                position.x = -100 + i;
                position.y = -100 + j;
                tileSpawnCords[i, j] = position;
                GameObject tile = Instantiate<GameObject>(tilePrefab);
                tile.transform.position = position;
                tile.transform.SetParent(TILEANCHOR);
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void MenuOpen()
    {
        if(mode == GameMode.playing)
        {
            mode = GameMode.idle;
            EscPanelFind.SetActive(true);
        }
        else
        {
            mode = GameMode.playing;
            EscPanelFind.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Texture2D groundTiles;

    [Header("Set Dynamically")]
    public Sprite[] tileTextures;
    public GameObject berezkaRodnayaPrefab;

    protected SpriteRenderer spriteRend;

    private void Awake()
    {
        tileTextures = new Sprite[8];
        tileTextures = Resources.LoadAll<Sprite>(groundTiles.name);
        spriteRend = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        int randomSpriteNum = Random.Range(0, 8);
        spriteRend.sprite = tileTextures[randomSpriteNum];
        float spawnChance = Random.Range(0f, 1f);
        if(spawnChance >= 0.997f)
        {
            GameObject birchSpawn = Instantiate<GameObject>(berezkaRodnayaPrefab);
            birchSpawn.transform.position = transform.position;
            birchSpawn.transform.SetParent(gameObject.transform);
            float chanceOfTransform = Random.Range(0f, 1f);
            birchSpawn.transform.localScale = Vector3.one * chanceOfTransform;
            if(chanceOfTransform >= 0.5f)
            {
                transform.rotation = Quaternion.Euler(0f, 360 * chanceOfTransform, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsFading : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float dungLifeDuration;
    public float fadingSpeed;
    public Color fadingColor;
    public bool Dung;

    [Header("Set Dynamically")]
    public Color OriginColor;
    public bool fading;

    protected SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (Dung)
        {
            Invoke("DungDeath", dungLifeDuration);
        }
    }

    void Start()
    {
        OriginColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dung)
        {
            if (fading)
            {
                OriginColor = Color.Lerp(OriginColor, fadingColor, 0.5f);
                //if (sprite.color.a <= 100f) OriginColor = fadingColor;
            }
            else
            {
                OriginColor.a += fadingSpeed * Time.deltaTime;
            }
            sprite.color = OriginColor;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        fading = true;
    }
    private void OnTriggerExit(Collider other)
    {
        fading = false;
    }

    void DungDeath()
    {
        Destroy(gameObject);
    }
}

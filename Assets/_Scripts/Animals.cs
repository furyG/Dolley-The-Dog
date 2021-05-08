using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [Header("Set in Inspector: Animals")]
    public string typeOfAnimal;
    public int speed;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;
    public float runningFromDogSpeed;
    public float coloringTime;
    public float dieTime;

    [Header("Set Dynamically: Animals")]
    public float timeNextDecision = 0;
    public bool chosen;
    public Rigidbody rigid;
    public Color red;
    public Color originColor;
    public bool outTheHerd;
    public float outTime;
    public GameObject findExclamation;
    public GameObject dungPrefab;
    //public int facing = 0;

    private Neighborhood neighborhood;

    protected static Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };
    protected Animator anim;
    protected SpriteRenderer sRend;
    protected HerdCenter hrdCntrScrpt;
    protected GameObject herdCenterGO;
    protected SphereCollider sphereColl;

    protected void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
        sphereColl = GetComponent<SphereCollider>();
        findExclamation = transform.GetChild(0).gameObject;
        findExclamation.SetActive(false);

        pos = Random.insideUnitCircle* HerdSpawner.S.spawnRadius;

        Vector3 vel = Random.onUnitSphere * HerdSpawner.S.velocity;
        rigid.velocity = vel;

        //LookAhead();
    }

    private void Start()
    {
        //в старте так как в herdSpawner.Awake() спавнит животных 
        //и дает им трансформ herdCenter
        hrdCntrScrpt = transform.parent.GetComponent<HerdCenter>();
        herdCenterGO = transform.parent.gameObject;

        red = Color.red;
        originColor = sRend.color;

        Invoke("SpawnDung", 4f);
    }

    void FixedUpdate()
    {
        if(DolleytheDog.D.mode == DolleytheDog.GameMode.playing)
        {
            Vector3 vel = rigid.velocity;

            #region herdVectors
            Vector3 velAvoid = Vector3.zero;
            Vector3 tooClosePos = neighborhood.avgClosePos;
            if (tooClosePos != Vector3.zero)
            {
                velAvoid = pos - tooClosePos;
                velAvoid.Normalize();
                velAvoid *= HerdSpawner.S.velocity;
            }

            Vector3 velAlign = neighborhood.avgVel;

            if (velAlign != Vector3.zero)
            {
                velAlign.Normalize();
                velAlign *= HerdSpawner.S.velocity;
            }

            Vector3 velCenter = neighborhood.avgPos;
            if (velCenter != Vector3.zero)
            {
                velCenter -= transform.position;
                velCenter.Normalize();
                velCenter *= HerdSpawner.S.velocity;
            }

            Vector3 delta = HerdCenter.POS - pos;
            bool attracted = (delta.magnitude > HerdSpawner.S.attractPushDist);
            Vector3 velAttract = delta.normalized * HerdSpawner.S.velocity;

            float fdt = Time.fixedDeltaTime;
            if (velAvoid != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAvoid, HerdSpawner.S.collAvoid * fdt);
            }
            else
            {
                if (velAlign != Vector3.zero)
                {
                    vel = Vector3.Lerp(vel, velAvoid, HerdSpawner.S.velMatching * fdt);
                }
                if (velCenter != Vector3.zero)
                {
                    vel = Vector3.Lerp(vel, velAlign, HerdSpawner.S.flockCentering * fdt);
                }
                if (velAttract != Vector3.zero)
                {
                    if (attracted)
                    {
                        vel = Vector3.Lerp(vel, velAttract, HerdSpawner.S.attractPull * fdt);
                    }
                    else
                    {
                        vel = Vector3.Lerp(vel, velAttract, HerdSpawner.S.attractPush * fdt);
                    }
                }
            }
            vel = vel.normalized * HerdSpawner.S.velocity;
            rigid.velocity = vel;
            #endregion herdVectors

            Vector3 deltaС = pos - herdCenterGO.transform.position;
            if (deltaС.magnitude >= HerdSpawner.S.spawnRadius)
            {
                findExclamation.SetActive(true);
                SetTime();
                sRend.color = Color.Lerp(originColor, red, Time.fixedDeltaTime / coloringTime);
                coloringTime -= Time.fixedDeltaTime;
                if (coloringTime <= Time.fixedDeltaTime)
                {
                    coloringTime = 1f;
                }

                if (Time.time - dieTime >= outTime)
                {
                    anim.CrossFade(typeOfAnimal + "_1_Dead", 0);
                    Invoke("Death", 1.5f);
                }

                //Debug.Log("" + deltaС.magnitude);
            }
            else
            {
                outTheHerd = false;
                findExclamation.SetActive(false);
                sRend.color = originColor;
            }
        }
        if (chosen)
        {
            sRend.color = red;
            Vector3 kowmarVector = Random.insideUnitSphere * (HerdSpawner.S.spawnRadius+1) * 0.08f;
            pos += new Vector3(kowmarVector.x, kowmarVector.y, 0f);
        }
    }

    void OnCollisionStay(Collision coll)
    {
        if(coll.gameObject.tag == "Animal")
        {
            Vector3 delta = transform.position - coll.transform.position;
            delta.Normalize();
            transform.position += delta * Time.deltaTime * runningFromDogSpeed;
            //boxColl.enabled = false;
            //Debug.Log("zadela korova drygyu korovy");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Dog")
        {
            chosen = false;
            anim.CrossFade(typeOfAnimal + "_Walk_0", 0);
            Vector3 delta = transform.position - other.transform.position;
            delta.Normalize();
            transform.position += delta * Time.deltaTime * runningFromDogSpeed;
        }
    }

    void SpawnDung()
    {
        float chance = Random.Range(0f, 1f);
        if(chance > 0.9f)
        {
            GameObject dungSpawn = Instantiate<GameObject>(dungPrefab);
            //dungSpawn.transform.SetParent(gameObject.transform);
            dungSpawn.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
        else
        {
            Invoke("SpawnDung", 4f);
        }
    }

    void SetTime()
    {
        if (!outTheHerd)
        {
            outTime = Time.time;
            outTheHerd = true;
        }
    }
    void Death()
    {
        sphereColl.enabled = false;
        gameObject.SetActive(false);
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}

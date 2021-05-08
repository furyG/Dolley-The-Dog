using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    static public Dog D;

    [Header("Set In Inspector")]
    public float speed;
    public Sprite DogStaying;
    public float timeOfBarking;
    public float timeSlowingDung;

    [Header("Set Dynamically")]
    public int dirHeld = -1;
    public float originSpeed;
    public Vector3 mousePix;
    public Vector3 startPos;
    public GameObject findBark;
    public bool barking = false;
    public float lastBark;
    public bool inDung;

    private Rigidbody rigid;
    private Animator anim;
    private SpriteRenderer sprite;

    public Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    void Awake()
    {
        D = this;
        originSpeed = speed;

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        findBark = transform.GetChild(0).gameObject;
        findBark.SetActive(false);
    }

    void Start()
    {
        startPos = Vector3.zero;
    }

    void Update()
    {
        if (DolleytheDog.D.mode == DolleytheDog.GameMode.playing)
        {
            rigid.isKinematic = true;
            Vector3 pos = transform.position;
            dirHeld = -1;

            mousePix = Input.mousePosition;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePix);

            Vector3 moveVel = new Vector3(mousePos.x - pos.x, mousePos.y - pos.y, startPos.z);

            Vector3 delta = pos - mousePos;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                rigid.isKinematic = false;
                rigid.velocity += moveVel * speed;

                //Debug.Log(" " + rigid.velocity);
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    delta.x = (delta.x > 0) ? dirHeld = 2 : dirHeld = 0;
                }
                else
                {
                    delta.y = (delta.y > 0) ? dirHeld = 3 : dirHeld = 1;
                }
            }
            if (Input.GetKey(KeyCode.Mouse1) && !barking)
            {
                SetTimeBark();
                findBark.SetActive(true);
            }
            if (lastBark <= Time.time)
            {
                findBark.SetActive(false);
                barking = false;
            }

            //анимация
            if (dirHeld == -1)
            {
                //anim.CrossFade("Dog_Walk_4", 0);
                anim.speed = 0;
            }
            else
            {
                anim.CrossFade("Dog_Walk_" + dirHeld, 0);
                anim.speed = Mathf.Abs(Vector3.Magnitude(delta)) * 0.3f;
            }
            //Debug.Log(" " + Vector3.Magnitude(delta));
            if (inDung)
            {
                anim.CrossFade("Dog_Stunned", 0f);
                speed /= 25f;
            }
            else
            {
                speed = originSpeed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Dung")
        {
            originSpeed = speed;
            FallInDung();
            Destroy(collision.gameObject);
        }
    }
    void SetTimeBark()
    {
        if (!barking)
        {
            lastBark = Time.time + timeOfBarking;
            barking = true;
        }
    }
    public void FallInDung()
    {
        if (!inDung)
        {
            anim.CrossFade("Dog_Stunned", 0f);
            inDung = true;
            Invoke("FallInDung", timeSlowingDung);
        }
        else
        {
            inDung = false;
        }
    }
}

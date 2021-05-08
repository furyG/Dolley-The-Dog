using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndPanel : MonoBehaviour
{
    [Header("Set Dynamically")]
    public float cordY;
    public int A;
    public int N;
    public Vector3 cords;
    public Text alive, dead, necessary, aliveF, LvlNum;
    public Image D, L;

    protected RectTransform pos;


    private void Awake()
    {
        pos = GetComponent<RectTransform>();
        cords = pos.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (DolleytheDog.D.EachLevelChecking && cords.y > -126)
        {
            cords.y -= 70f * Time.deltaTime;
        }
        else if(!DolleytheDog.D.EachLevelChecking && cords.y < 50)
        {
            cords.y += 70f * Time.deltaTime;
        }
        pos.anchoredPosition = cords;

        alive.text = ""+DolleytheDog.D.aAlive;
        aliveF.text = alive.text;
        dead.text = "" + DolleytheDog.D.aDead;
        necessary.text = "" + DolleytheDog.D.aNecessary;
        LvlNum.text = "" + DolleytheDog.D.levelNumber;

        A = int.Parse(alive.text);
        N = int.Parse(necessary.text);
        if (A >= N)
        {
            L.gameObject.SetActive(false);
            D.gameObject.SetActive(true);
        }
        else
        {
            D.gameObject.SetActive(false);
            L.gameObject.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animals
{
    public bool staying = false;
    public string[] CowAnimStaying = new string[4];
    void Update()
    {
        if(DolleytheDog.D.mode == DolleytheDog.GameMode.playing)
        {
            if (hrdCntrScrpt.mode == HerdCenter.herdMode.move)
            {
                staying = false;
                rigid.isKinematic = false;
                rigid.velocity = directions[HerdCenter.facing] * speed;
                anim.CrossFade("Cow_Walk_" + HerdCenter.facing, 0);
                anim.speed = 1;
            }
            else if (hrdCntrScrpt.mode == HerdCenter.herdMode.idle && !staying)
            {
                ChooseMove();
            }
        }
    }
    public void ChooseMove()
    {
        staying = true;
        float chance = Random.Range(0f, 1f);
        if(chance > 0.4f)
        {
            rigid.isKinematic = true;
            int chooseAnimation = Random.Range(0, 3);
            anim.CrossFade(CowAnimStaying[chooseAnimation], 0);
        }
        else
        {
            rigid.isKinematic = false;
        }
    }
}

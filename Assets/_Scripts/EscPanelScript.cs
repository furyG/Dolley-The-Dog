using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPanelScript : MonoBehaviour
{
    void Update()
    {
        if(DolleytheDog.D.mode == DolleytheDog.GameMode.idle)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

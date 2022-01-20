using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDeath : MonoBehaviour
{
    public Stats Stats = new Stats();
    void VidaAumento()
    {
        Collider[] Compaņeros = Physics.OverlapSphere(transform.position, Stats.Range);

        foreach (var compaņeros_ in Compaņeros)
        {
            if (compaņeros_ != null)
            {

                if (compaņeros_.gameObject.layer == 10)
                {
                    Player Compaņero = compaņeros_.gameObject.GetComponent<Player>();
                    if (Compaņero != null && Compaņero.Stats.team == Stats.team)
                    {
                        Stats.Objetivo = Compaņero.gameObject;
                        Compaņero.Stats.vidacurrent += Compaņero.Stats.vidacurrent + 10;
                        return;
                    }
                }
            }
        }
    }
}

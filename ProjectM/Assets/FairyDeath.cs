using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDeath : MonoBehaviour
{
    public Stats Stats = new Stats();
    void VidaAumento()
    {
        Collider[] Compa�eros = Physics.OverlapSphere(transform.position, Stats.Range);

        foreach (var compa�eros_ in Compa�eros)
        {
            if (compa�eros_ != null)
            {

                if (compa�eros_.gameObject.layer == 10)
                {
                    Player Compa�ero = compa�eros_.gameObject.GetComponent<Player>();
                    if (Compa�ero != null && Compa�ero.Stats.team == Stats.team)
                    {
                        Stats.Objetivo = Compa�ero.gameObject;
                        Compa�ero.Stats.vidacurrent = Compa�ero.Stats.vidacurrent + 10;
                        return;
                    }
                }
            }
        }
    }
}

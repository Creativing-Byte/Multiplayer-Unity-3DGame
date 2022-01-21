using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDeath : MonoBehaviour
{
    public Stats Stats = new Stats();
    void VidaAumento()
    {
        Collider[] Compañeros = Physics.OverlapSphere(transform.position, Stats.Range);

        foreach (var compañeros_ in Compañeros)
        {
            if (compañeros_ != null)
            {

                if (compañeros_.gameObject.layer == 10)
                {
                    Player Compañero = compañeros_.gameObject.GetComponent<Player>();
                    if (Compañero != null && Compañero.Stats.team == Stats.team)
                    {
                        Stats.Objetivo = Compañero.gameObject;
                        Compañero.Stats.vidacurrent = Compañero.Stats.vidacurrent + 10;
                        return;
                    }
                }
            }
        }
    }
}

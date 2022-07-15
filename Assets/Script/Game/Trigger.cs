using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public string Team;
    public bool Active;
    public GameObject Direccion, Direccion1;

    private float timer = 0;

    public const int TIME = 5;

    void OnTriggerStay(Collider collider)
    {
        if((timer -= Time.deltaTime) > 0)
            return;
        timer = TIME;

        if (Direccion == null && collider.gameObject.TryGetComponent(out Tower t) && t.Stats.team != Team)
        {
            Direccion = collider.gameObject;
        }

        Player Player = collider.gameObject.GetComponent<Player>();
        if (Player != null)
        {
            Stats Stats = Player.Stats;
            if (Stats != null && Player != null)
            {
                if (Stats.team == Team)
                {
                    if (Direccion != null)
                    {
                        if (Player.Destino == null)
                        {
                            Player.Destino = Direccion;
                            Player.DestinoTarget = Direccion.transform.position;
                            Player.DestinoTarget.y = Direccion.transform.position.y;
                        }
                    }
                    else
                    {
                        if (Player.Destino == null)
                        {
                            if (Direccion1 != null)
                            {
                                Player.Destino = Direccion1;
                                Player.DestinoTarget = Direccion1.transform.position;
                                Player.DestinoTarget.y = Direccion1.transform.position.y;
                            }
                        }
                    }
                }
            }
        }
    }
}

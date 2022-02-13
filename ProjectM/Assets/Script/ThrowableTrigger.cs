using System;
using UnityEngine;

public class ThrowableTrigger : MonoBehaviour
{
    public string ObjectName;
    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.name == ObjectName)
        {
            if (ObjectName== "Velociraptor(Clone)")
            {
                collider.gameObject.GetComponent<Velociraptors>().Active();
                Destroy(gameObject);
            }
            else if (ObjectName== "ZarigueyaPrefacInstancia(Clone)")
            {
                collider.gameObject.GetComponent<ZarigueyaPrefac>().Active();
                Destroy(gameObject);
            }
            else if (ObjectName == "FireballLanzador(Clone)")
            {
                collider.gameObject.GetComponent<FireballPrefac>().Active();
               
            }
            else if (ObjectName == "DragonGigante(Clone)")
            {
                collider.gameObject.GetComponent<DragonGigante>().Active();

            }
            else if (ObjectName == "HachasPrefac(Clone)")
            {
                collider.gameObject.GetComponent<AxesPrefac>().Active();

            }

        }
        if (collider.gameObject.name == "FireBallBall(Clone)")
        {
            collider.gameObject.GetComponent<FireballPersonaje>().Vfx();
            Destroy(gameObject);
        }
        if (collider.gameObject.name == "Axe(Clone)")
        {
            collider.gameObject.GetComponent<Axes>().Vfx();
            Destroy(gameObject);
        }
        if (collider.gameObject.name == "FireDragonGiGANTE(Clone)")
        {
            collider.gameObject.GetComponent<DragonGigantePersonaje>().Vfx();
            Destroy(gameObject);
        }
    }
}

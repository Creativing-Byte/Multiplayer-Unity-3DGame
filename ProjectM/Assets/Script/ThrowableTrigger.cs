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
                Destroy(gameObject);
            }

        }
    }
}

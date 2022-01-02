using System;
using UnityEngine;

public class ThrowableTrigger : MonoBehaviour
{
    public string ObjectName;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == ObjectName)
        {
            collider.gameObject.GetComponent<Velociraptors>().Active();
            Destroy(gameObject);
        }
    }
}

using System;
using UnityEngine;

public class WizardCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            try
            {
                if (other.gameObject.GetComponent<Player>().Stats.team != GetComponentInParent<Player>().Stats.team)
                {
                    GetComponentInParent<Wizard>().enemiesInRange++;
                }
            }
            catch (NullReferenceException)
            {
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            try
            {
                if (other.gameObject.GetComponent<Player>().Stats.team != GetComponentInParent<Player>().Stats.team)
                {
                    GetComponentInParent<Wizard>().enemiesInRange--;
                }
            }
            catch (NullReferenceException)
            {
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoArea : MonoBehaviour
{
    public Tower Stats;
    public GameObject Fireball;
    // Start is called before the first frame update
    void Start()
    {
        Stats = GetComponent<Tower>();
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("toer2");
        Fireball = other.gameObject;
        if (Fireball.name == "FireBallPrefac(Clone)")
        {
            Debug.Log("toer3");
            if (other.gameObject.GetComponent<FirebaPlayer>().team != Stats.Stats.team)
            {
                Stats.Stats.vidacurrent -= 50;
            }
        }
    }

    /*public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "DañoArea")
            {
                if (other.gameObject.GetComponent<FirebaPlayer>().team != Stats.team)
                {
                    Stats.vidacurrent -= 50;
                }
                else if (other.gameObject.GetComponent<Fireball>().StatsP.team != Stats.team)
                {
                    Stats.vidacurrent -= 5;
                }
            }
        }*/
}

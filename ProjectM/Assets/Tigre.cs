using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tigre : Player
{
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("AnimalAttack", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LucyWarrior : Player 
{
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("FloorPunch", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

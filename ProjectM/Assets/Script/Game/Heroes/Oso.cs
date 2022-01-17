using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Oso : Player
{
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("AnimalAttack", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

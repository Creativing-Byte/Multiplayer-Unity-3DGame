using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Anubis : Player
{
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("AnubisPunch", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

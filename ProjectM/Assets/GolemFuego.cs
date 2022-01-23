using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class GolemFuego : Player
{
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("TorchGiantPunch", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

using Photon.Pun;
using UnityEngine.AI;

public class Velociraptor : Player
{
    void Awake()
    {
        Stats.team = GetComponentInParent<Velociraptors>().Stats.team;
        Stats.vidamax = GetComponentInParent<Velociraptors>().Stats.vidamax;
        Stats.vidacurrent = GetComponentInParent<Velociraptors>().Stats.vidacurrent;
        Stats.ataque = GetComponentInParent<Velociraptors>().Stats.ataque;
        Stats.velocidad = GetComponentInParent<Velociraptors>().Stats.velocidad;
        Stats.vataque = GetComponentInParent<Velociraptors>().Stats.vataque;
        Stats.Range = GetComponentInParent<Velociraptors>().Stats.Range;
        Stats.Objetivo = GetComponentInParent<Velociraptors>().Stats.Objetivo;
        Stats.TipoGround = GetComponentInParent<Velociraptors>().Stats.TipoGround;
        Stats.TipoFlying = GetComponentInParent<Velociraptors>().Stats.TipoFlying;
        Stats.Ground = GetComponentInParent<Velociraptors>().Stats.Ground;
        Stats.Flying = GetComponentInParent<Velociraptors>().Stats.Flying;

        GetComponent<NavMeshAgent>().stoppingDistance = Stats.Range - 1f;
    }
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("AnimalAttack", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}

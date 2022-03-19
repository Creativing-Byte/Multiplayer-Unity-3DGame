using Photon.Pun;
using UnityEngine;


public class FirebaPlayer : MonoBehaviour
{
    public GameObject target;
    public string team;
    // Start is called before the first frame update
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1);
    }


}

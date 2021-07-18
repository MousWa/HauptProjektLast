using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Aiming : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
   public static Vector2 targtPos;
   public GameObject AimingC;

    // Update is called once per frame

    void Start()
    {
        PhotonNetwork.Instantiate(AimingC.name, targtPos, Quaternion.identity);
    }
    void Update()
    {
        if (photonView.IsMine)
        {

            targtPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = targtPos;
            

        }
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            targtPos = (Vector3)stream.ReceiveNext();
        }
    }
}

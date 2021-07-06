using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Aiming : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    Vector2 targtPos;
  

    // Update is called once per frame
    void Update()
    {
        targtPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = targtPos;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
   

    // Update is called once per frame
   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhotonNetwork.LoadLevel(3);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PhotonNetwork.LoadLevel(5);
        }
    }
    public void StartLevel2()
    {
        PhotonNetwork.LoadLevel(2);
    }
    public void StartLevel4()
    {
        PhotonNetwork.LoadLevel(4);
      
    }
 

}

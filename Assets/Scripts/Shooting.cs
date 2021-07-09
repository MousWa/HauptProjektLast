using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Shooting : MonoBehaviourPun

     
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject fairpoint;
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }
        }
        public void Shoot() {
       PhotonNetwork.Instantiate(bullet.name, fairpoint.transform.position, Quaternion.identity);

    
    }
}

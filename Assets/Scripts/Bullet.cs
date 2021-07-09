using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Bullet : MonoBehaviourPun
{
    // Start is called before the first frame update
    public float speed;
    public float destroyTime;
    public GameObject Aim;
    private Vector2 target;

    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("OnDestroy", RpcTarget.AllBuffered);

    }


    // Update is called once per frame
    void Update()
    {
        target  = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.Translate(Aim.transform.position * Time.fixedDeltaTime * speed);
    }
    [PunRPC]
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}

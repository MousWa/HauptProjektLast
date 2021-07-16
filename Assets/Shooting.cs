using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Shooting : MonoBehaviourPunCallbacks
{
    private Vector3 targtPos;
    private float shootingTimer = 0.0f;
    private Vector3 direction;
    public GameObject BulletPrefab;
    public Transform firePoint;
    public float buzlletSpeed;
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            targtPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ProcessInputs();
        }

    }
    private void ProcessInputs()
    {

       
        if (Input.GetButton("Fire1") && shootingTimer <= 0.0)
        {

            shootingTimer = 0.2f;
            photonView.RPC("Fire", RpcTarget.All);
        }

        if (shootingTimer > 0.0f)
        {
            shootingTimer -= Time.deltaTime;
        }

    }
    [PunRPC]
    public void Fire()
    {


        GameObject bullet;
        bullet = Instantiate(BulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 bulletpos = bullet.GetComponent<Transform>().position;
        direction = (targtPos - bulletpos).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * buzlletSpeed, targtPos);
     
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targtPos);


        }
        else if (stream.IsReading)
        {
            targtPos = (Vector3)stream.ReceiveNext();

        }
    }

}

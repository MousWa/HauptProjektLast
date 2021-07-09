using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerMovements : MonoBehaviourPunCallbacks,IPunObservable
{
    public PhotonView pv;
    private SpriteRenderer sp;
    public float moveSpeed = 50;
    public int jumpforce = 50;

    private bool IsGrounded;
    private Rigidbody2D rb;
    private Vector3 smoothMove;
    public GameObject gun;
   
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
     
        if (photonView.IsMine)
        {
            ProcessInputs();
        }
        else
        {
            smoothMovement();
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
        
    }

    private void ProcessInputs()
    {
        float h = Input.GetAxis("Horizontal");

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.A)) {
            sp.flipX = true;
            gun.GetComponent<SpriteRenderer>().flipX = true;

            pv.RPC("OnDirectionChange_Left", RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            sp.flipX = false;
            gun.GetComponent<SpriteRenderer>().flipX = false;

            pv.RPC("OnDirectionChange_Right",RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }
       
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (photonView.IsMine)
        {
            if (c.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
        }
    }
    [PunRPC]
    void OnDirectionChange_Left()
    {
        sp.flipX = true;
        gun.GetComponent<SpriteRenderer>().flipX = true;
    }


    [PunRPC]
    void OnDirectionChange_Right()
    {
        sp.flipX = false;
        gun.GetComponent<SpriteRenderer>().flipX = false;

    }
    void OnCollisionExit2D(Collision2D c)
    {
        if (photonView.IsMine)
        {
            if (c.gameObject.tag == "Ground")
            {
                IsGrounded = false;
            }
        }
    }
    void Jump()
    {
      
        rb.AddForce(Vector2.up * jumpforce);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }
    }
}

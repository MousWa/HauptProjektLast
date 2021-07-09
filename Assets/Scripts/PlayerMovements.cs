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
    public SpriteRenderer gun;
    Boolean isRotat = false;
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
            if (!isRotat)
            { 

            transform.Rotate(new Vector3(0, 180, 0));
                isRotat = true;
                pv.RPC("OnDirectionChange_Left", RpcTarget.Others);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isRotat)
            {
                Debug.Log("why is not rotat");
                transform.Rotate(new Vector3(0, 180, 0));
                isRotat = false;
                pv.RPC("OnDirectionChange_Right", RpcTarget.Others);
            }
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
        if (!isRotat)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            isRotat = true;
        }
    }


    [PunRPC]
    void OnDirectionChange_Right()
    {
        if (isRotat)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            isRotat = false;
        }
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

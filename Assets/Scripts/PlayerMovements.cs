using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerMovements : MonoBehaviourPunCallbacks,IPunObservable
{
    public PhotonView pv;

    public float moveSpeed = 50;
    public int jumpforce = 50;

    private bool IsGrounded;
    private Rigidbody2D rb;
    private Vector3 smoothMove;
    public GameObject crosshair;
    Vector2 mousPos;
    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        

    }
    private void Update()
    {
        mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
    void FixedUpdate() {
      
    }
    private void ProcessInputs()
    {
   
      
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move*moveSpeed, rb.velocity.y);
       

        if (Input.GetKeyDown(KeyCode.Space)&& IsGrounded)
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

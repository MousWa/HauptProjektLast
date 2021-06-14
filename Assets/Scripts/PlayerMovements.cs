using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerMovements : MonoBehaviourPunCallbacks,IPunObservable
{
    public PhotonView pv;

    public int moveSpeed = 50;
    public int jumpforce = 50;

    private bool IsGrounded;
    private Rigidbody2D rb;
    private Vector3 smoothMove;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!animator)
        {
            return;
        }
     
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
        animator.SetBool("IsRun", h != 0);
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

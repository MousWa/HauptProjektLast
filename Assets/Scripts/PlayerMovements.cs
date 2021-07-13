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
    public static bool isRotat = false;

    private Transform aimTrans;


    private void Awake()
    {
        aimTrans = transform.Find("Aim");
    }

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
        pisolRotation();
        pv.RPC("pisolRotationPUN", RpcTarget.Others);
        
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
    [PunRPC]
    void pisolRotationPUN()
    {
        
        Vector3 mousePosition = GetMousPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle >= -70 && angle <= 40 && !isRotat)
        {
            aimTrans.eulerAngles = new Vector3(0, 0, angle);
            Debug.Log(angle);
        }

        if (angle >= 110 && angle <= 220 && isRotat)
        {
            aimTrans.eulerAngles = new Vector3(0, 0, angle);
            Debug.Log(angle);
        }
    }

    void pisolRotation()
    {

        Vector3 mousePosition = GetMousPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle >= -70 && angle <= 40 && !isRotat)
        {
            aimTrans.eulerAngles = new Vector3(0, 0, angle);
            Debug.Log(angle);
        }

        if (angle >= 110 && angle <= 220 && isRotat)
        {
            aimTrans.eulerAngles = new Vector3(0, 0, angle);
            Debug.Log(angle);
        }
    }
    public static Vector3 GetMousPosition()
    {
        Vector3 vec = GetMousPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    private static Vector3 GetMousPositionWithZ(Vector3 mousePosition, Camera main)
    {
        Vector3 worldPosition = main.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }
}

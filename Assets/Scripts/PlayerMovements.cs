using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviourPunCallbacks,IPunObservable
{

    public Image healthbarImage;
    public PhotonView pv;
    private SpriteRenderer sp;
    public float moveSpeed = 50;
    public int jumpforce = 50;
    public float buzlletSpeed;
    private float shootingTimer = 0.0f;
    private bool IsGrounded;
    private Rigidbody2D rb;
    private Vector3 smoothMove;
    public SpriteRenderer gun;
    public static bool isRotat = false;
    public GameObject BulletPrefab;
    private Transform aimTrans;
    private Vector3 targtPos;
    private Vector3 direction ;
    public float RotationSpeed = 90.0f;
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;
   // private bool controllable = true;
    public Transform firePoint;
    private float rotation = 0.0f;
    private float acceleration = 0.0f;
    const float maxHealth = 100f;
    float currentHelath = maxHealth;
    public float Damage;
  

    private void Awake()
    {
        aimTrans = transform.Find("Aim");
        
       
    }

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
       


    }
    private void Update()
    {
        
        if (photonView.IsMine)
        {
            targtPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            ProcessInputs(); 
            if (currentHelath <= 0f)
            {
                PhotonNetwork.LoadLevel(2);
            }

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
        /*
        if (Input.GetKeyDown(KeyCode.A)) {
            if (!isRotat&&photonView.IsMine)
            { 
                transform.Rotate(new Vector3(0, 180, 0));
                isRotat = true;
                pv.RPC("OnDirectionChange_Left", RpcTarget.Others);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isRotat && photonView.IsMine)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isRotat = false;
                pv.RPC("OnDirectionChange_Right", RpcTarget.Others);
            }
        }*/
        if (Input.GetKeyDown(KeyCode.Space) /*&& IsGrounded*/)
        {
            Jump();
        }
        if (Input.GetButton("Fire1") && shootingTimer <= 0.0 )
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
        /** Use this if you want to fire one bullet at a time **/
        bullet= Instantiate(BulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 bulletpos = bullet.GetComponent<Transform>().position;
        direction = (targtPos - bulletpos).normalized;
        //bulletrb.AddForceAtPosition(direction*buzlletSpeed, targtPos) ;
        bullet.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * buzlletSpeed, targtPos);
        /** Use this if you want to fire two bullets at once **/
        //Vector3 baseX = rotation * Vector3.right;
        //Vector3 baseZ = rotation * Vector3.forward;

        //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
        //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if (photonView.IsMine)
        {
            if (c.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
            if (c.gameObject.tag == "Bullet")
            {
                currentHelath -= Damage;
                healthbarImage.fillAmount = currentHelath / maxHealth;
                pv.RPC("RPCTakeDamage", RpcTarget.All);
            }
        }
    }
    /*
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
    */
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
            stream.SendNext(targtPos);
            stream.SendNext(currentHelath);

        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
            targtPos = (Vector3)stream.ReceiveNext();
            currentHelath= (float)stream.ReceiveNext(); ;

        }
    }

   
    [PunRPC]
    void RPCTakeDamage()
    {
        if (!pv.IsMine)
        {
            return;
        }
        currentHelath -= Damage;
        healthbarImage.fillAmount = currentHelath / maxHealth;

        if (currentHelath <= 0)
        {
            Die();
        }
    }
    void die()
    {
        if (currentHelath <= 0f)
        {


            //Destroy(gameObject);

            //PhotonNetwork.LoadLevel(2);
        }
    }
    [PunRPC]
    void Die()
    {
            if (currentHelath <= 0f)
            {
            
            
                //Destroy(gameObject);
                
                //PhotonNetwork.LoadLevel(2);
            }
    }
    /*
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
        pv.RPC("pisolRotationPUN", RpcTarget.Others);
    }*/

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

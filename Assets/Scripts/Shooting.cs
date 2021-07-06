using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPerfab;
    public float buzlletSpeed;
    public GameObject Aim;

    // Update is called once per frame
    void Update()
    {
        rotaion();
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
          
        }
    }
    void Shoot()
    {
        Transform aim = Aim.GetComponent<Transform>();
        GameObject bullet = Instantiate(bulletPerfab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(aim.position * buzlletSpeed, ForceMode2D.Impulse);

    }
    private void rotaion() {

        Vector3 aim = Aim.transform.position;
        Vector3 aimDirection = (aim - transform.position).normalized;
        float angel = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angel);
    }
}
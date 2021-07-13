using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{
    private static bool isRotate = false;
    private Transform aimTrans;
    

    private void Awake()
    {
        aimTrans = transform.Find("Aim");
    }

    // Update is called once per frame
    void Update()
    {
        IsRotat();
        Vector3 mousePosition = GetMousPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle >= -70 && angle <= 40 && !isRotate)
        {
            aimTrans.eulerAngles = new Vector3(0, 0, angle);
            Debug.Log(angle);
           

        }
        
        if (angle >= 110 && angle <= 220 && isRotate)
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

    public static bool IsRotat()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isRotate = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRotate = false;
        }
        return isRotate;
    }
}

using UnityEngine;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{

    public static float PistolDamage;
  
    public void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}

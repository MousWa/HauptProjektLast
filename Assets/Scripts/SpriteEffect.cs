using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
         
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
            Color newColor = new Color(
                Random.value,
                Random.value,
                Random.value
                );
            spriteRenderer.color = newColor;
        
    }
}

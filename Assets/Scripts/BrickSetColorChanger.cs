using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSetColorChanger : MonoBehaviour
{
    public Component[] spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        foreach ( SpriteRenderer rend in spriteRenderer)
            rend.color = new Color(Random.value, Random.value, Random.value);
        
    }
}

    




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickColorChanger : MonoBehaviour
{
    SpriteRenderer rend;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = new Color(Random.value, Random.value, Random.value);
    }
}






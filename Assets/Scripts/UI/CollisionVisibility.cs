using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVisibility : MonoBehaviour
{
    Transform playerTransform;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > playerTransform.position.z)
        {
            spriteRenderer.sortingOrder = 0;
        }
        else
        {
            spriteRenderer.sortingOrder = 3;
        }
    }
}

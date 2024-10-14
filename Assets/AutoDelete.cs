using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    [SerializeField] public Vector2Int nodeXY;
    [SerializeField] public float timeToDelete;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeToDelete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    public Vector2 xAxisLimit;
    public Vector2 zAxisLimit;

    [SerializeField] private Transform target;

    void Awake()
    {
        target = GameObject.Find("Player").transform;
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, xAxisLimit.x, xAxisLimit.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, zAxisLimit.x, zAxisLimit.y);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
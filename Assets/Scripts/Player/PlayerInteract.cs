using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact Properties")]
    [SerializeField] float checkSphereRadius = 3f;
    private Collider closestCollider;
    public static event Action<PlayerInput, Interactable> SetInteractText;

    [Header("Interact Variables")]
    public bool canInteract;
    bool canShowInteractionText = true;

    [Header("Debug")]
    [SerializeField] bool debugInfo;

    void FixedUpdate()
    {
        closestCollider = null;
        if(canShowInteractionText)
        {
            CheckClosestCollider();

            if (closestCollider != null)
            {
                SetInteractText?.Invoke(GetComponent<PlayerInput>(), closestCollider.GetComponent<Interactable>());
            }
            else
            {
                SetInteractText?.Invoke(GetComponent<PlayerInput>(), null);
            }
        }
    }

    void CheckClosestCollider()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkSphereRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Interactable>() != null)
            {
                if (closestCollider == null)
                {
                    closestCollider = hitCollider;
                }
                else
                {
                    if (Vector3.Distance(transform.position, closestCollider.transform.position) > Vector3.Distance(transform.position, hitCollider.transform.position))
                    {
                        closestCollider = hitCollider;
                    }
                }
            }
        }
    }

    public void StartInteraction()
    {
        closestCollider.GetComponent<Interactable>().BaseInteract();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if(debugInfo)
        {
            Gizmos.DrawWireSphere(transform.position, checkSphereRadius);    
        }
    }
}
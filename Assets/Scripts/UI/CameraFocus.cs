using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float opacityChangeTime;
    Collider affectedObject;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        affectedObject = null;
    }

    void LateUpdate()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(target.position));
        RaycastHit raycastHit;

        Physics.Raycast(ray, out raycastHit);

        if(raycastHit.collider != null && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Collision") && raycastHit.collider.transform.GetComponentInChildren<SpriteRenderer>())
        {
            if(affectedObject != null && raycastHit.collider != affectedObject)
            {
                StartCoroutine(ChangeOpacity(affectedObject, true));
                StartCoroutine(ChangeOpacity(raycastHit.collider, false));
                affectedObject = raycastHit.collider;
            }
            else if(affectedObject == null)
            {
                // Color newColor = 
                //raycastHit.collider.transform.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,0.3f);
                StartCoroutine(ChangeOpacity(raycastHit.collider, false));
                affectedObject = raycastHit.collider;
            }
        }
        else
        {
            if(affectedObject != null)
            {
                //affectedObject.transform.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,1f);
                StartCoroutine(ChangeOpacity(affectedObject, true));
                affectedObject = null;
            }
        }
    }

    IEnumerator ChangeOpacity(Collider collider, bool intendedVisibility)
    {
        if(intendedVisibility)
        {
            for(float x = 0.3f; x <= 1f; x += 0.01f)
            {
                collider.transform.GetComponentInChildren<SpriteRenderer>().color = new Color(x,x,x,x);
                yield return new WaitForSeconds(opacityChangeTime);
            }
        }
        else
        {
            for(float x = 1f; x >= 0.3f; x -= 0.01f)
            {
                collider.transform.GetComponentInChildren<SpriteRenderer>().color = new Color(x,x,x,x);
                yield return new WaitForSeconds(opacityChangeTime);
            }
        }
    }
}

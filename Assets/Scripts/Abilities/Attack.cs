using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //public LayerMask layerMask; -- isso aqui ja tava comentado 

    public bool isProjectile;
    public int damageAmount;
    [SerializeField] protected bool isAttacking;
    public Animator animator;
    public float fireForce;
    public float projectileDuration;

    protected List<Collider> colliders;
    protected List<Collider> usedColliders;
    
    // protected ContactFilter2D contactFilter2D;-- isso aqui ja tava comentado 

    public void Awake()
    {
        colliders = new List<Collider>();
        usedColliders = new List<Collider>();
        // contactFilter2D = new ContactFilter2D
        // {
        //     layerMask = this.layerMask,
        //     useLayerMask = true
        // };

        isAttacking = false;

        animator = GetComponent<Animator>(); 
    }

    public virtual void ExecuteAttack()
    {
        usedColliders = new List<Collider>();
        isAttacking = true;
    }

    protected virtual void DealDamage(Collider collider)
    {

    }

    public virtual void StopAttack()
    {
        isAttacking = false;
        gameObject.SetActive(false);
    }
}

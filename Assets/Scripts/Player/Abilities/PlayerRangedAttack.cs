using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRangedAttack", menuName = "Data/Player/Abilities/RangedAttack")]
public class PlayerRangedAttack : PlayerAbility
{
    [Header("Player Ranged Attack Statistics")]
    [SerializeField] GameObject projectileBase;
    [SerializeField] List<Projectile> projectiles;

    public float projectileSpawnHeight;
    public bool needActivation;
    
    public override void Activate()
    {
        base.Activate();
        GetComponents();

        Vector3 targetPoint = GetTarget();
        ChangePlayerOrientation(targetPoint);

        int projectileIndex = 0;

        foreach(Projectile projectile in projectiles)
        {
            GameObject instanciatedProjectile = InstantiateAttack();
            SetAttackProperties(instanciatedProjectile, targetPoint, projectileIndex);

            projectileIndex++;
        }
    }

    Vector3 GetTarget()
    {
        Vector3 targetPoint = playerInpuHandler.GetPlayerAimVector();
        return targetPoint;
    }

    GameObject InstantiateAttack()
    {
        Vector3 projectilePosition = player.transform.position + new Vector3(0, projectileSpawnHeight, 0);
        return Instantiate(projectileBase, projectilePosition, Quaternion.identity, attacksParentGameObject);
    }

    void SetAttackProperties(GameObject instanciatedProjectile, Vector3 targetPoint, int projectileIndex)
    {
        Attack actualAttack = instanciatedProjectile.GetComponent<Attack>();

        Projectile currentProjectileInformation = projectiles[projectileIndex];

        //projectile damage
        actualAttack.damageAmount = currentProjectileInformation.damage;
        
        //projectile launch - fire force and angle
        Vector3 attackDirection = (targetPoint - player.transform.position).normalized;
        attackDirection = Quaternion.AngleAxis(currentProjectileInformation.spreadAngle, Vector3.forward) * attackDirection;
        instanciatedProjectile.GetComponent<Rigidbody>().AddForce(attackDirection * currentProjectileInformation.fireForce, ForceMode.Impulse);

        //projetcile duration
        Destroy(instanciatedProjectile, currentProjectileInformation.duration);
    }

    void ChangePlayerOrientation(Vector3 targetPoint)
    {
        playerOrientation.ChangeOrientation(targetPoint);
    }

    [Serializable] public struct Projectile 
	{
		public int damage;
        public float duration;
        public float fireForce;
        public float size;

        public float spreadAngle;

		public Projectile(int _damage, float _duration, float _fireForce, float _size, float _spreadAngle) 
		{
			damage = _damage;
            duration = _duration;
            fireForce = _fireForce;
            size = _size;

            spreadAngle = _spreadAngle;
		}
	}
}

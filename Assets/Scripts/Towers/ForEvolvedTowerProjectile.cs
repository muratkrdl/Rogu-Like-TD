using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEvolvedTowerProjectile : MonoBehaviour
{
    [SerializeField] Projectile projectile;

    [SerializeField] int code;

    Vector2 offset;

    void Update()
    {
        if(projectile.GetTarget == null || GameStateManager.Instance.GetIsGamePaused) return;
        
        offset = projectile.GetTarget.position - transform.position;
        offset.Normalize();
        float rot_z = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public void SpawnDamage()
    {
        DamagerObjPool.Instance.GetDamagerObj(code).SetValues(projectile.GetDamage, projectile.GetDamageType, transform.position);
    }

}

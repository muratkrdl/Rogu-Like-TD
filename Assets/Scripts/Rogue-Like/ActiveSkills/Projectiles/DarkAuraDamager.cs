using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAuraDamager : SkillProjectileDamagerBaseClass
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.CompareTag(TagManager.TOWER) && other.TryGetComponent<TowerInfoKeeper>(out var towerInfoKeeper) && InventorySystem.Instance.GetSkillSO(GetSkillCode).isEvolved)
        {
            towerInfoKeeper.SetExtraDamageFromDarkAura(1.25f);
        }
    }
}

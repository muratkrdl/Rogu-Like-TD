using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BloodRain : ActiveSkillBaseClass
{
    Color spriteColor = new(1, .1f, .1f, 1);

    void Start() 
    {
        UseSkill().Forget();
        InventorySystem.Instance.OnNewSkillGain += InventorySystem_OnNewSkillGain;
        InventorySystem.Instance.OnSkillUpdate += InventorySystem_OnSkillUpdate;
        InventorySystem.Instance.OnSkillEvolved += InventorySystem_OnSkillEvolved;
        SubInventoryCDEvent();
    }

    async UniTaskVoid UseSkill()
    {
        await UniTask.WaitUntil(() => GetCanUseSkill);
        while (true)
        {
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill(), cancellationToken: GetCTS.Token);
            await UniTask.Delay(TimeSpan.FromSeconds(GetSkillCoolDown()));
            
            Skill();
        }
    }

    void Skill()
    {
        if(!GlobalUnitTargets.Instance.CanPlayerUseSkill()) return;

        StopAllCoroutines();
        StartCoroutine(nameof(SkillCDSlider));

        var projectile = ActiveSkillProjectileObjectPool.Instance.GetProjectile(1);
        projectile.GetComponent<Animator>().SetTrigger(ConstStrings.RESET);
        projectile.GetComponent<SpriteRenderer>().color = spriteColor;
        projectile.GetComponent<SkillProjectileDamagerBaseClass>().SetDamageOnSpawn();
        projectile.transform.position = transform.position;
        projectile.transform.localScale = GetCurrentScale;
    }

    protected override void EvolveSkill()
    {
        // tower Impact
        spriteColor = new(.6f, 0, 0, 1);
    }

    void OnDestroy() 
    {
        InventorySystem.Instance.OnNewSkillGain -= InventorySystem_OnNewSkillGain;
        InventorySystem.Instance.OnSkillUpdate -= InventorySystem_OnSkillUpdate;
        InventorySystem.Instance.OnSkillEvolved -= InventorySystem_OnSkillEvolved;
        UnSubInventoryCDEvent();
        OnDestroy_CancelCTS();
    }
    
}

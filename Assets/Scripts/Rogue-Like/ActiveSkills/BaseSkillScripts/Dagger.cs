using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Dagger : ActiveSkillBaseClass
{
    int projectileCode = 2;

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

            Skill().Forget();
        }
    }

    async UniTaskVoid Skill()
    {
        if(!GlobalUnitTargets.Instance.CanPlayerUseSkill()) return;

        StopAllCoroutines();
        StartCoroutine(nameof(SkillCDSlider));

        for(int i = 0; i < GetCurrentProjectileAmount; i++)
        {
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill(), cancellationToken: GetCTS.Token);
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill(), cancellationToken: GetCTS.Token);

            var projectile = ActiveSkillProjectileObjectPool.Instance.GetProjectile(projectileCode);
            
            SoundManager.Instance.PlaySound2D(ConstStrings.DAGGER);

            Vector2 newLookPos;
            if(GetComponentInParent<GetInputs>().GetMoveInput == Vector2.zero)
            {
                newLookPos = GetComponentInParent<GetInputs>().GetLastMoveDir;
            }
            else
            {
                newLookPos = GetComponentInParent<GetInputs>().GetMoveInput;
            }
            
            projectile.GetComponent<DaggerDamager>().ClearList();
            projectile.GetComponent<SkillProjectileDamagerBaseClass>().SetDamageOnSpawn();
            projectile.SetMoveableProjectile(newLookPos, transform.position, true);
        }
    }

    protected override void EvolveSkill()
    {
        projectileCode = 6;
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

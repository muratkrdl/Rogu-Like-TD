using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireBall : ActiveSkillBaseClass
{
    void Start() 
    {
        UseSkill().Forget();
        InventorySystem.Instance.OnNewSkillGain += InventorySystem_OnNewSkillGain;
    }

    async UniTaskVoid UseSkill()
    {
        await UniTask.WaitUntil(() => GetCanUseSkill);
        while (true)
        {
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill());
            await UniTask.Delay(TimeSpan.FromSeconds(GetSkillCoolDown()));

            Skill().Forget();
        }
    }

    async UniTaskVoid Skill()
    {
        if(!GlobalUnitTargets.Instance.CanPlayerUseSkill()) return;

        for(int i = 0; i < InventorySystem.Instance.GetSkillSO(GetSkillCode).ProjectileCount; i++)
        {
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill());
            await UniTask.Delay(TimeSpan.FromSeconds(.2f));
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill());

            var projectile = ActiveSkillProjectileObjectPool.Instance.GetProjectile(3);
            
            Vector2 extraForce = Vector2.zero;
            Vector2 newLookPos;
            if(GetComponentInParent<GetInputs>().GetMoveInput == Vector2.zero)
            {
                newLookPos = GetComponentInParent<GetInputs>().GetLastMoveDir;
            }
            else
            {
                newLookPos = GetComponentInParent<GetInputs>().GetMoveInput;
            }

            if(Mathf.Abs(newLookPos.x) > Mathf.Abs(newLookPos.y))
            {
                extraForce.y = UnityEngine.Random.Range(-.15f, .15f);
            }
            else
            {
                extraForce.x = UnityEngine.Random.Range(-.15f, .15f);
            }

            projectile.GetComponent<FireballDamager>().ClearList();
            projectile.SetMoveableProjectile(newLookPos + extraForce, transform.position, true);
            projectile.SetBaseClassValues(InventorySystem.Instance.GetSkillSO(GetSkillCode).Value, GetIsEvolved);
        }
    }

    void OnDestroy() 
    {
        InventorySystem.Instance.OnNewSkillGain -= InventorySystem_OnNewSkillGain;
    }

}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Vine : ActiveSkillBaseClass
{
    [SerializeField] Animator vineRotateAnimator;
    [SerializeField] Animator vinePosAnimator;

    [SerializeField] SpriteRenderer[] vineSprites;
    [SerializeField] Sprite evolvedSprite;

    CancellationTokenSource cts = new();

    void Start() 
    {
        UseSkill().Forget();
        InventorySystem.Instance.OnNewSkillGain += InventorySystem_OnNewSkillGain;
        InventorySystem.Instance.OnSkillUpdate += InventorySystem_OnSkillUpdate;
        InventorySystem.Instance.OnSkillEvolved += InventorySystem_OnSkillEvolved;
    }

    async UniTaskVoid UseSkill()
    {
        await UniTask.WaitUntil(() => GetCanUseSkill);
        while (true)
        {
            await UniTask.WaitUntil(() => GlobalUnitTargets.Instance.CanPlayerUseSkill());
            await UniTask.Delay(TimeSpan.FromSeconds(GetSkillCoolDown()), cancellationToken: cts.Token);

            Skill();
        }
    }

    protected override void OnSkillUpdateFunc()
    {
        vinePosAnimator.SetTrigger((GetCurrentProjectileAmount).ToString());
    }

    void Skill()
    {
        if(!GlobalUnitTargets.Instance.CanPlayerUseSkill()) return;

        vineRotateAnimator.SetTrigger(ConstStrings.ANIM);
    }

    protected override void EvolveSkill()
    {
        cts.Cancel();
        foreach(var item in vineSprites)
        {
            item.sprite = evolvedSprite;
            item.GetComponent<BoxCollider2D>().size = new(.25f, .25f);
        }
        vineRotateAnimator.SetTrigger(ConstStrings.ACTIVE_SKILLS_FULL);
    }

    void OnDestroy() 
    {
        InventorySystem.Instance.OnNewSkillGain -= InventorySystem_OnNewSkillGain;
        InventorySystem.Instance.OnSkillUpdate -= InventorySystem_OnSkillUpdate;
        InventorySystem.Instance.OnSkillEvolved -= InventorySystem_OnSkillEvolved;
    }
    
}

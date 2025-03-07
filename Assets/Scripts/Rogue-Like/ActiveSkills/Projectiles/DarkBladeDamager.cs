using UnityEngine;

public class DarkBladeDamager : SkillProjectileDamagerBaseClass
{
    TrailRenderer trailRenderer;

    void Start() 
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();    
    }

    public void AnimEvent_ClearTrailRenderer()
    {
        trailRenderer.Clear();
    }

    protected override void EvolveFunc(Collider2D other)
    {
        GlobalUnitTargets.Instance.GetPlayerTarget().GetComponent<PlayerHealth>().SetHP(-1, DamageType.truedamage);
    }

    public void PlaySFX()
    {
        SoundManager.Instance.PlaySound2D(ConstStrings.DARKBLADE);
    }
}

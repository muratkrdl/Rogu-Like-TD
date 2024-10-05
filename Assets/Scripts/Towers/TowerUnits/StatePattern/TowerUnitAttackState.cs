using UnityEngine;

public class TowerUnitAttackState : ITowerUnitState
{
    public void EnterState(TowerUnitValues unitValues)
    {
        Debug.Log("Entering Attack");

        unitValues.IsAttacking = true;
        unitValues.IsChasing = true;
        unitValues.GetTowerUnitAttack().Attack().Forget();
    }

    public void ExitState(TowerUnitValues unitValues)
    {
        unitValues.GetTowerUnitAnimator().ResetTrigger(ConstStrings.UNIT_ANIMATOR_ATTACK);
        unitValues.GetTowerUnitStateController().ClearTokenSource();
        unitValues.IsAttacking = false;
        unitValues.IsChasing = false;

        Debug.Log("Exiting Attack");
    }

    public void UpdateState(TowerUnitValues unitValues)
    {
        if(unitValues.GetTowerUnitSetTarget().GetCurrentTarget.TryGetComponent<UnitValues>(out var component))
        {
            if(component.IsDead)
            {
                unitValues.GetTowerUnitStateController().ChangeState(new TowerUnitIdleState());
            }
        }
        else
        {
            unitValues.GetTowerUnitStateController().ChangeState(new TowerUnitIdleState());
        }

        Debug.Log("Updating Attack");
    }
}

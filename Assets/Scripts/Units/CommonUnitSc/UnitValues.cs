using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class UnitValues : MonoBehaviour
{
    [SerializeField] Rigidbody2D unitRigidBody;

    [SerializeField] UnitSO unitSO;
    [SerializeField] Transform projectileOutPos;

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] UnitAnimator unitAnimator;
    [SerializeField] UnitMove unitMove;
    [SerializeField] UnitAttack unitAttack;
    [SerializeField] UnitStateController unitStateController;
    [SerializeField] UnitHealth unitHealth;
    [SerializeField] UnitFlashFX unitFlashFX;

    [SerializeField] EnemySetTarget enemySetTarget;
    [SerializeField] GuardSetTarget guardSetTarget;

    [SerializeField] bool isEnemy;
    [SerializeField] bool isSpecial;
    [SerializeField] bool isBoss;
    [SerializeField] bool hasLongRange;
    [SerializeField] DamageType damageType;

    [SerializeField] Transform towerBasePosition;

    Vector2 plusDamageRange;

    float initialMoveSpeed;

    bool isChasing = false;
    bool isAttacking = false;
    bool isDead = false;
    bool isGoingToRight;
    bool isWaiting = true;
    bool speedChanged = false;

    public float GetInitialMoveSpeed
    {
        get => initialMoveSpeed;
    }
    public Vector2 PlusDamageRange
    {
        get => plusDamageRange;
        set => plusDamageRange = value;
    }
    public Transform TowerBasePosition
    {
        get => towerBasePosition;
        set => towerBasePosition = value;
    }
    public UnitSO UnitSO
    {
        get => unitSO;
        set
        {
            unitSO = value;
            if(isSpecial)
            {
                unitHealth.CurrenHealth = 10;
                return;
            }
            unitHealth.CurrenHealth = unitSO.MaxHealth;
        }
    }
    public Transform GetProjectileOutPos
    {
        get => projectileOutPos;
    }
    public DamageType GetDamageType
    {
        get => damageType;
    }

    public bool IsChasing
    {
        get => isChasing;
        set => isChasing = value;
    }
    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }
    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
    public bool IsGoingToRight
    {
        get => isGoingToRight;
        set => isGoingToRight = value;
    }
    public bool IsWaiting
    {
        get => isWaiting;
        set => isWaiting = value;
    }
    public bool IsSpeedChanged
    {
        get => speedChanged;
        set => speedChanged = value;
    }
    public bool GetIsEnemy
    {
        get => isEnemy;
    }
    public bool GetIsSpecial
    {
        get => isSpecial;
    }
    public bool GetIsBoss
    {
        get => isBoss;
    }
    public bool GetHasLongRange
    {
        get => hasLongRange;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        unitRigidBody.velocity = Vector2.zero;
        return unitRigidBody;
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }
    public UnitAnimator GetUnitAnimator()
    {
        return unitAnimator;
    }
    public UnitMove GetUnitMove()
    {
        return unitMove;
    }
    public EnemySetTarget GetEnemySetTarget()
    {
        return enemySetTarget;
    }
    public GuardSetTarget GetGuardSetTarget()
    {
        return guardSetTarget;
    }
    public UnitAttack GetUnitAttack()
    {
        return unitAttack;
    }
    public UnitStateController GetUnitStateController()
    {
        return unitStateController;
    }
    public UnitHealth GetUnitHealth()
    {
        return unitHealth;
    }
    public UnitFlashFX GetUnitFlashFX()
    {
        return unitFlashFX;
    }

    void Start() 
    {
        initialMoveSpeed = navMeshAgent.speed;  

        GameStateManager.Instance.OnPause += GameStateManager_OnPause;
        GameStateManager.Instance.OnResume += GameStateManager_OnResume;
    }

    void GameStateManager_OnPause(object sender, EventArgs e)
    {
        navMeshAgent.enabled = false;
    }

    void GameStateManager_OnResume(object sender, EventArgs e)
    {
        navMeshAgent.enabled = true;
    }

    public void SetValues(Transform spawnPos, UnitSO unitSO, int code)
    {
        isDead = false;
        if(isEnemy)
            isWaiting = false;
        else
            isWaiting = true;
        
        navMeshAgent.Warp(spawnPos.position);
        UnitSO = unitSO;
        unitStateController.StartFunc(code);
        unitAnimator.SetTrigger(ConstStrings.RESET);

        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void ResetAllValues()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void ForSpecialEnemies()
    {
        isWaiting = true;
        if(isEnemy)
            navMeshAgent.Warp(GlobalUnitTargets.Instance.GetEnemyWaitPos.position);
    }

    public void SetUnitSpeed(float amount)
    {
        navMeshAgent.speed = amount;
        GetComponent<Animator>().speed = amount;
    }

    public void SetUnitSpeedBaseValue()
    {
        navMeshAgent.speed = initialMoveSpeed;
        GetComponent<Animator>().speed = 1;
    }

    public async UniTaskVoid StunSlowUnit(float value, float stunTime)
    {
        speedChanged = true;
        SetUnitSpeed(value);
        await UniTask.Delay(TimeSpan.FromSeconds(stunTime));
        speedChanged = false;
        SetUnitSpeedBaseValue();
    }

    public void PlaySFX()
    {
        GetComponent<AudioSource>().Play();
    }

    void OnDestroy() 
    {
        GameStateManager.Instance.OnPause -= GameStateManager_OnPause;
        GameStateManager.Instance.OnResume -= GameStateManager_OnResume;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemyKeeper : MonoBehaviour
{
    public EventHandler<OnEnemyListKeeperChangedEventArgs> OnEnemyListKeeperChanged;
    public class OnEnemyListKeeperChangedEventArgs : EventArgs
    {
        public bool isIn;
        public Transform enemyTransform;
    }

    [SerializeField] CircleCollider2D circleCollider2D;

    List<Transform> enemiesInRange = new();

    public List<Transform> GetEnemiesInRangeList
    {
        get => enemiesInRange;
    }

    public void SetNewCollider(float amount)
    {
        circleCollider2D.radius = amount / 2;
    }

    public Transform GetClosestEnemy()
    {
        Transform closestEnemy = enemiesInRange[0];
        foreach(Transform item in enemiesInRange)
        {
            if(Mathf.Abs(Vector2.Distance(item.position, transform.position)) < Mathf.Abs(Vector2.Distance(closestEnemy.position, transform.position)))
            {
                closestEnemy = item;
            }
        }

        return closestEnemy;
    }

    public bool ItemInList(Transform unit)
    {
        foreach (var item in enemiesInRange)
        {
            if(item == unit)
                return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag(TagManager.ENEMY))
        {
            if(!enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Add(other.transform);
                OnEnemyListKeeperChanged?.Invoke(this, new() { isIn = true, enemyTransform = other.transform } );
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag(TagManager.ENEMY))
        {
            if(enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Remove(other.transform);
                OnEnemyListKeeperChanged?.Invoke(this, new() { isIn = false, enemyTransform = other.transform } );
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoKeeper : MonoBehaviour
{
    [SerializeField] EvolvedBuildAnim evolvedBuildAnim;

    [SerializeField] TowerHealth towerHealth;

    [SerializeField] Image rangeCircle;

    TowerInfoSo currentTowerInfo;

    int currentTowerLevel = 1;
    int currentTowerCode = -1;

    int clickedTowerCode;

    public int ClickedTowerCode
    {
        get => clickedTowerCode;
        set => clickedTowerCode = value;
    }

    public int CurrentTowerLevel
    {
        get => currentTowerLevel;
        set => currentTowerLevel = value;
    }

    public TowerInfoSo GetCurrentTowerInfo
    {
        get => currentTowerInfo;
    }

    public int GetCurrentTowerCode
    {
        get => currentTowerCode;
    }
    
    public void SetCurrentTowerInfo(int i, int level)
    {
        currentTowerInfo = AllTowerInfos.Instance.GetTowerInfoSo(i, level);
        currentTowerCode = i;
        rangeCircle.transform.localScale = new(currentTowerInfo.Range, currentTowerInfo.Range);
        GetComponentInChildren<TowerEnemyKeeper>().SetNewCollider(currentTowerInfo.Range);
        towerHealth.SetTowerHealth(GetCurrentTowerInfo.maxHealth);
    }

    public void ResetAllValues()
    {
        currentTowerInfo = null;
        currentTowerLevel = 1;
        currentTowerCode = -1;
        clickedTowerCode = 0;
        rangeCircle.transform.localScale = Vector2.zero;
        towerHealth.ResetHealthPoints();
    }

    public EvolvedBuildAnim GetEvolvedBuildAnim()
    {
        return evolvedBuildAnim;
    }

}

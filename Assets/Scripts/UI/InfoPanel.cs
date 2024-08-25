using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel Instance;

    public EventHandler<OnClickedTowerInfoEventArg> OnClickedTowerInfo;
    public class OnClickedTowerInfoEventArg : EventArgs
    {
        public TowerInfoSo towerInfoSo1;
        public bool isMainTower;
    }

    [SerializeField] Animator animator;

    [SerializeField] Image infoIcon;
    [SerializeField] TextMeshProUGUI infoName;
    [SerializeField] TextMeshProUGUI heartText;
    [SerializeField] TextMeshProUGUI damageText;

    [SerializeField] Image sellButton;
    [SerializeField] TextMeshProUGUI sellPriceText;
    [SerializeField] Image getInOutButton;

    TowerInfoSo currentTowerInfoSO;

    void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        OnClickedTowerInfo += InfoPanel_OnClickedTowerInfo;
    }

    void OnDestroy() 
    {
        OnClickedTowerInfo -= InfoPanel_OnClickedTowerInfo;
    }

    void InfoPanel_OnClickedTowerInfo(object sender, OnClickedTowerInfoEventArg e)
    {
        if(e.towerInfoSo1 == null)
        {
            SetInfoPanelAnim(false);
            return;
        }
        else if(currentTowerInfoSO == e.towerInfoSo1 && animator.GetBool(ConstStrings.INFO_PANEL_ANIMATOR_ISIN))
        {
            return;
        }

        currentTowerInfoSO = e.towerInfoSo1;

        SetInfoPanelAnim(true);

        infoIcon.sprite = e.towerInfoSo1.towerImageIcon;
        infoName.text = e.towerInfoSo1.Name;
        // heartText.text = getcomponent currenthealth
        damageText.text = e.towerInfoSo1.BaseDamageRange.x.ToString() + "-" + e.towerInfoSo1.BaseDamageRange.y.ToString();
        damageText.color = e.towerInfoSo1.DamageTypeColor;

        sellButton.gameObject.SetActive(!e.isMainTower);
        getInOutButton.gameObject.SetActive(e.isMainTower);
        sellPriceText.text = e.towerInfoSo1.sellPrice.ToString();
    }

    public void OnClick_InOut()
    {
        MainTowerManager.Instance.OnInteractWithMainTower?.Invoke(this, EventArgs.Empty);
    }

    public void SetInfoPanelAnim(bool value)
    {
        if(!value && animator.GetBool(ConstStrings.INFO_PANEL_ANIMATOR_ISIN))
        {
            animator.SetBool(ConstStrings.INFO_PANEL_ANIMATOR_ISIN, value);
        }
        else if(value && !animator.GetBool(ConstStrings.INFO_PANEL_ANIMATOR_ISIN))
        {
            animator.SetBool(ConstStrings.INFO_PANEL_ANIMATOR_ISIN, value);
        }
    }

}

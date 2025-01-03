using System;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    public static ExperienceSystem Instance;

    public EventHandler<OnGetExperienceEventArgs> OnGetExperience;
    public class OnGetExperienceEventArgs : EventArgs
    {
        public string name;
    }

    [SerializeField] ExperiencePanel experiencePanel;

    [SerializeField] int[] gemEarnXPAmount;
    [SerializeField] int[] needExperienceToLevelUp;

    int currentExperience = 0;
    int currentLevel = 1;

    void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        OnGetExperience += ExperienceSystem_OnGetExperience;
        experiencePanel.SetExperienceSlider(currentExperience, needExperienceToLevelUp[currentLevel-1], currentLevel);
    }

    void ExperienceSystem_OnGetExperience(object sender, OnGetExperienceEventArgs e)
    {
        int increaseAmount = e.name switch
        {
            "ExperienceObj1(Clone)" => gemEarnXPAmount[0],
            "ExperienceObj2(Clone)" => gemEarnXPAmount[1],
            _ => gemEarnXPAmount[2],
        };

        increaseAmount += PermanentSkillSystem.Instance.GetPermanentSkillSO(2).Value;

        IncreaseExperience(increaseAmount);
    }

    void IncreaseExperience(int increaseAmount)
    {
        currentExperience += increaseAmount;
        if(currentExperience >= needExperienceToLevelUp[currentLevel-1])
        {
            currentLevel++;
            currentExperience -= needExperienceToLevelUp[currentLevel-2];
            SetRandomUISkillButtons(false);
        }
        experiencePanel.SetExperienceSlider(currentExperience, needExperienceToLevelUp[currentLevel-1], currentLevel);
    }

    public void SetRandomUISkillButtons(bool isBoss)
    {
        LevelUpPanel.Instance.SetRandomUISkillButtons(isBoss);
        SoundManager.Instance.PlaySound2D(ConstStrings.LEVELUP);
    }

    void OnDestroy() 
    {
        OnGetExperience -= ExperienceSystem_OnGetExperience;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkilPanellButton : MonoBehaviour
{
    [Header("직접 넣어야 하는것")]
    public GameObject SkillPointView;
    public GameObject RaceSkill;
    public GameObject JobSkill;
    public GameObject WeaponSkill;


    [VisibleEnum(typeof(SkillType2))]
    public void SkillButton(int sk)
    {        
        AllClose();
        switch (sk)
        {
            case (int)SkillType2.All:
                SkillPointManager.ins.AllSkillButton();
                break;
            case (int)SkillType2.Passive:
                SkillPointManager.ins.PassiveSkillButton();
                break;
            case (int)SkillType2.Active:
                SkillPointManager.ins.ActiveSkillButton();
                break;
            default:
                break;
        }

    }

    public void RaceSkillButton()
    {
        AllClose();
        RaceSkill.SetActive(true);
    }

    public void ClassSkillButton()
    {
        AllClose();
    }
    public void WeaponSkillButton()
    {
        AllClose();
        WeaponSkill.SetActive(true);
    }

    public void JobSkillButton()
    {
        AllClose();
        
        if(SkillPointManager.ins.JobLevelButtonIndex!=-1)
        {
            SkillPointManager.ins.DisplaySkillPointSlot_job();
        }

        JobSkill.SetActive(true);
    }

    public void AllClose()
    {
        SkillPointView.SetActive(false);
        RaceSkill.SetActive(false);
        JobSkill.SetActive(false);
        WeaponSkill.SetActive(false);
        SkillPointManager.ins.SkillLevelUpPage.SetActive(false);
    }
}

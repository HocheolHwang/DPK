using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public SkillSystem SkillSystem { get; set; }
    // Start is called before the first frame update
    ISkill[] skill = new ISkill[8];
    public void Start()
    {
        SkillSystem = GameObject.Find("@Scene").GetComponent<SkillSystem>();
        //skill[0] = new SkillWarrior.TestSkill1();
        //skill[1] = new SkillWarrior.TestSkill1();
        //skill[2] = new SkillWarrior.TestSkill1();
        //skill[3] = new SkillWarrior.TestSkill1();
        //skill[4] = new SkillWarrior.TestSkill1();
        //skill[5] = new SkillWarrior.TestSkill1();
        //skill[6] = new SkillWarrior.TestSkill1();
        //skill[7] = new SkillWarrior.TestSkill1();
        // for(int i = 0; i < skill.Length; i++)
        // {
        //     skill[i] = new SkillWarrior.TestSkill1();
        // }
    }

    public void SelectSkill(Define.SkillKey key)
    {
        // string s = skill[(int)key].animationName;
        //Debug.Log($"Skill Key = {key}");
        //Debug.Log($"Skill Key = {(int)key}");
        //skill[(int)key].InitSetting();
        //Debug.Log($"{skill[(int)key]}");
        //skill[(int)key].Using();
        switch (key)
        {
            case Define.SkillKey.Q:
                SkillSystem.currentType = Define.SkillType.Target;
                break;
            case Define.SkillKey.W:
                SkillSystem.currentType = Define.SkillType.Range;
                break;
        }
        

    }

    public void CastSkill(Define.SkillKey key)
    {
        //string s = skill[(int)key].animationName;
        Debug.Log($"Skill Key = {key}");
    }



    public void Clear()
    {
        Debug.Log("Skill System Cleared");
        SkillSystem.Clear();
    }
}

/*
public class SkillSlot : MonoBehaviour
{
    // Start is called before the first frame update
    SOSkill[] skill = new SOSkill[8];
    public void Start()
    {
        for(int i = 0; i < skill.Length; i++)
        {
            skill[i] = new SOSkill();
        }
    }

    public void CastSkill(Define.SkillKey key)
    {
        string s = skill[(int)key].animationName;
        Debug.Log($"Skill Key = {key}");
    }
}
*/
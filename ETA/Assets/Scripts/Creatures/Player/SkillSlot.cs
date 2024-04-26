using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    // Start is called before the first frame update
    ISkill[] skill = new ISkill[8];
    public void Start()
    {
        skill[0] = new SkillWarrior.TestSkill1();
        skill[1] = new SkillWarrior.TestSkill1();
        skill[2] = new SkillWarrior.TestSkill1();
        skill[3] = new SkillWarrior.TestSkill1();
        skill[4] = new SkillWarrior.TestSkill1();
        skill[5] = new SkillWarrior.TestSkill1();
        skill[6] = new SkillWarrior.TestSkill1();
        skill[7] = new SkillWarrior.TestSkill1();
        // for(int i = 0; i < skill.Length; i++)
        // {
        //     skill[i] = new SkillWarrior.TestSkill1();
        // }
    }

    public void CastSkill(Define.SkillKey key)
    {
        // string s = skill[(int)key].animationName;
        Debug.Log($"Skill Key = {key}");
        Debug.Log($"Skill Key = {(int)key}");
        skill[(int)key].InitSetting();
        Debug.Log($"{skill[(int)key]}");
        skill[(int)key].Using();
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
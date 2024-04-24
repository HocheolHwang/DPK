using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

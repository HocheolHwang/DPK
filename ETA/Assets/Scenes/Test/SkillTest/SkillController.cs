using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public ISkill skill;

    // Start is called before the first frame update
    void Start()
    {
        SkillWarrior.TestSkill1 testSkill1 = new SkillWarrior.TestSkill1();

        skill = testSkill1;
        skill.InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        skill.Using();
    }
}

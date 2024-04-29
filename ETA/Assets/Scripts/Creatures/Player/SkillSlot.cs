using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public SkillSystem SkillSystem { get; set; }
    // Start is called before the first frame update
    //ISkill[] skill = new ISkill[8];
    TmpSkill[] skill = new TmpSkill[8];
    private Animator _animator;
    public void Start()
    {
        SkillSystem = GetComponent<SkillSystem>();

        string[] loadedSkills = { "TargetSkill", "RangeSkill", "HoldSkill", "ImmediatelySkill", "GuardSkill"};
        for (int i = 0; i < loadedSkills.Length; i++)
        {
            string skillName = loadedSkills[i];
            Type type = Type.GetType(skillName);

            // Type이 유효하면 컴포넌트를 추가합니다.
            // 후에 as를 이용한 타입캐스트 해주기
            if (type != null && type.IsSubclassOf(typeof(Component)))
            {
                skill[i] = (TmpSkill)gameObject.AddComponent(type);
                
            }
        }
    }

    public void SelectSkill(Define.SkillKey key)
    {
        TmpSkill current = skill[(int)key];

        // 여기서 쿨타임 관리
        if (current.CanCastSkill() == false) return;

        switch (current.SkillType)
        {
            case Define.SkillType.Target:
                SkillSystem.currentType = Define.SkillType.Target;
                SkillSystem.SkillRange = current.skillRange;
                break;
            case Define.SkillType.Range:
                SkillSystem.currentType = Define.SkillType.Range;
                SkillSystem.SkillRange = current.skillRange;
                break;
            case Define.SkillType.Holding:
                SkillSystem.currentType = Define.SkillType.Holding;
                SkillSystem.SkillRange = current.skillRange;
                break;
            case Define.SkillType.Immediately:
                SkillSystem.currentType = Define.SkillType.Immediately;
                SkillSystem.SkillRange = current.skillRange;
                break;
        }


    }

    public void CancleSkill()
    {
        SkillSystem.currentType = Define.SkillType.None;

    }

    // 실제로 스테이트에서 발생하는 함수
    public void CastSkill(Define.SkillKey key)
    {
        //string s = skill[(int)key].animationName;
        skill[(int)key].Cast();
        Debug.Log($"Skill Key = {key}");
    }

    public void CastCollavoSkill(Define.SkillKey key)
    {
        skill[(int)key].CollavoCast();
    }

    public void NormalAttack()
    {
        GetComponent<WarriorNormalAttackSkill>().Cast();
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
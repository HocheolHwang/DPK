using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public SkillSystem SkillSystem { get; set; }
    // Start is called before the first frame update
    //ISkill[] skill = new ISkill[8];
    Skill[] skill = new Skill[8];
    public Skill[] Skills { get { return skill; } }

    private Animator _animator;

    private Skill _currentSkill;
    public Skill CurrentSkill { get { return _currentSkill; } }
    public void Start()
    {
        SkillSystem = GetComponent<SkillSystem>();
        // 내 캐릭터만 UI에 연결하기
        if(GetComponent<PhotonView>().IsMine) GameObject.FindObjectOfType<Dungeon_Popup_UI>().skillSlot = this;


        string[] loadedSkills = { "ArrowBomb", "ArrowShower", "ArrowStab", "WindShield", "Guard"};
        for (int i = 0; i < loadedSkills.Length; i++)
        {
            string skillName = loadedSkills[i];
            Type type = Type.GetType(skillName);

            // Type이 유효하면 컴포넌트를 추가합니다.
            // 후에 as를 이용한 타입캐스트 해주기
            if (type != null && type.IsSubclassOf(typeof(Component)))
            {
                skill[i] = (Skill)gameObject.AddComponent(type);
                
            }
        }
    }

    public void SelectSkill(Define.SkillKey key)
    {
        Skill current = skill[(int)key];

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
                SkillSystem.RangeType = current.RangeType;
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
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }
        _currentSkill = skill[(int)key];
        _currentSkill.Cast();
        Debug.Log($"Skill Key = {key}");
    }

    public void CastCollavoSkill(Define.SkillKey key)
    {
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }
        _currentSkill = skill[(int)key];
        _currentSkill.CollavoCast();
    }

    public void NormalAttack()
    {
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }
        // TODO: 직업이 여러개면 바꿔 주어야할 것
        if (gameObject.name.Contains("Warrior"))
        {
            _currentSkill = gameObject.GetOrAddComponent<WarriorNormalAttackSkill>();
        }
        else if(gameObject.name.Contains("Archer"))
        {
            _currentSkill = gameObject.GetOrAddComponent<ArcherNormalAttackSkill>();
        }
        else if (gameObject.name.Contains("Mage"))
        {
            _currentSkill = gameObject.GetOrAddComponent<MageNormalAttackSkill>();
        }
        else
        {
            Debug.Log("존재하지 않는 직업 입니다.");
        }
        
        _currentSkill.Cast();
    }


    public void Clear()
    {
        Debug.Log("Skill System Cleared");
        SkillSystem.Clear();
    }
}
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    protected PlayerController _controller;
    protected Animator _animator;
    protected SkillSystem _skillSystem;
    protected float _lastExcuteTime;
    protected float _cooldownTime;
    protected float _createTime;

    public Sprite skillIcon;
    public Define.SkillType SkillType = Define.SkillType.Target;
    public int Damage = 50;
    public Vector3 skillRange;
    public Vector3 CollavoSkillRange;
    public Define.RangeType RangeType;

    private Coroutine _currentCoroutine;
    public float ElapsedTime { get { return Time.time - _lastExcuteTime; } }
    public float CooldownTime { get { return _cooldownTime; } set { _cooldownTime = value; } }

    public string CollavoSkillName;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        //SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{ChangeCodeToName(classCode)}/{classSkills[classCode][i].skillName}");
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _skillSystem = GetComponent<SkillSystem>();
        
    }

    public void SetCoolDownTime(float time)
    {
        _cooldownTime = time;
        _lastExcuteTime = _cooldownTime * -1;
    }


    public void Cast()
    {
        _lastExcuteTime = Time.time;
        _currentCoroutine = Managers.Coroutine.Run(StartSkillCast());
    }

    public void CollavoCast()
    {
        _lastExcuteTime = Time.time;
        _currentCoroutine = StartCoroutine(StartCollavoSkillCast());
    }

    public bool CanCastSkill()
    {
        if (ElapsedTime >= _cooldownTime) return true;
        return false;
    }

    public abstract IEnumerator StartSkillCast();
    public virtual IEnumerator StartCollavoSkillCast() {
        yield return null;
    }

    public void StopCast()
    {
        if (_currentCoroutine == null) return;
        Managers.Coroutine.Stop(_currentCoroutine);
        
    }

    public void ChangeToPlayerMoveState()
    {
        if (this is not WarriorNormalAttackSkill && this is not ArcherNormalAttackSkill && this is not MageNormalAttackSkill)
        {
            _controller.SkillSlot.PreviousSkill = this;
        }
        if (_controller.photonView.IsMine) _controller.ChangeState(_controller.MOVE_STATE);
    }

    public void ChangeToMonsterMoveState()
    {
        if(PhotonNetwork.IsMasterClient) _controller.ChangeState(_controller.MOVE_STATE);
    }


}

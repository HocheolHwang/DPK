using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShield : Skill
{

    protected override void Init()
    {
        SetCoolDownTime(2);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/WindShield.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("GUARD", 0.05f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindShield, gameObject.transform);
        StartCoroutine(Evasion(1.0f));
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/GuardSkill");
        
        yield return new WaitForSeconds(0.9f);
        //Managers.Effect.Stop(ps);
        yield return new WaitForSeconds(0.1f);

        _controller.ChangeState(_controller.MOVE_STATE);
    }

    IEnumerator Evasion(float duration)
    {
        _controller.Evasion = true;
        yield return new WaitForSeconds(duration);
        _controller.Evasion = false;
    }
}

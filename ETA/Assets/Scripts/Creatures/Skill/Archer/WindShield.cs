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
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindShield, 1.0f, gameObject.transform);
        Managers.Coroutine.Run(WindShieldCoroutine());
        Managers.Coroutine.Run(Evasion(1.0f));
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/ForestSpiritSpawn");
        
        yield return new WaitForSeconds(0.9f);
        //Managers.Effect.Stop(ps);
        yield return new WaitForSeconds(0.1f);


        ChangeToPlayerMoveState();
    }

    IEnumerator Evasion(float duration)
    {
        _controller.Evasion = true;
        yield return new WaitForSeconds(duration);
        _controller.Evasion = false;
    }

    IEnumerator WindShieldCoroutine()
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindShield, 1.0f, gameObject.transform);
        yield return new WaitForSeconds(0.1f);
    }
}

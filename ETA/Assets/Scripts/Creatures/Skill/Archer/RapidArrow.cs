using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidArrow : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Target;
        //RangeType = Define.RangeType.Square;
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/RapidArrow.png");
    }


    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL5", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);
        ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/RapidArrowHit").GetComponent<ParticleSystem>();
        ps1.transform.position = _skillSystem.TargetPosition - gameObject.transform.forward * 0.8f + gameObject.transform.up * 0.8f;
        ps1.Play();
        //Managers.Sound.Play("Skill/ArrowShowerHit");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;

        ParticleSystem ps02 = Managers.Resource.Instantiate("Effect/RapidArrowShot").GetComponent<ParticleSystem>();
        ps02.transform.position = transform.position + transform.forward * 0.8f + transform.up * 0.8f;

        ParticleSystem ps03 = Managers.Resource.Instantiate("Effect/RapidArrowCharge").GetComponent<ParticleSystem>();
        ps03.transform.position = transform.position + transform.forward / 2.0f + transform.up * 0.8f;

        //Managers.Sound.Play("Skill/ArrowShowerHit");
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.20f);
            ps03.Play();
            ps02.Play();
            Managers.Resource.Destroy(hitbox.gameObject);
            hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition;
            Managers.Sound.Play("Skill/ArrowShowerHit");
        }
        Managers.Resource.Destroy(ps1.gameObject);
        Managers.Resource.Destroy(ps02.gameObject);
        Managers.Resource.Destroy(ps03.gameObject);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidArrow : Skill
{
    [SerializeField] float _duration = 1.0f;
    [SerializeField] float _speed = 50.0f;
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Target;
        //RangeType = Define.RangeType.Square;
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ForestSpirit.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL5", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);


        ParticleSystem ps02 = Managers.Resource.Instantiate("Effect/RapidArrowHit").GetComponent<ParticleSystem>();
        ps02.transform.position = _skillSystem.TargetPosition + gameObject.transform.up/2;
        Managers.Sound.Play("Skill/TargetSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;

        ParticleSystem ps01 = Managers.Resource.Instantiate("Effect/RapidArrowShot").GetComponent<ParticleSystem>();
        //ps01.transform.rotation = hitbox.transform.rotation;
        ps01.transform.position = transform.position + transform.forward * 0.5f + transform.up;
        //hitbox.transform.position = _skillSystem.TargetPosition - transform.forward;

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.09f);
            ps01.Play();
            Managers.Resource.Destroy(hitbox.gameObject);
            ps02.Play();
            hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition;
            Managers.Sound.Play("Skill/TargetSkill");
        }
        Managers.Resource.Destroy(ps01.gameObject);
        Managers.Resource.Destroy(ps02.gameObject);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}

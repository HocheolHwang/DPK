using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBomb : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(8, 8, 8);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ArrowBomb.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);

        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/ArrowShot");

        yield return new WaitForSeconds(0.7f);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/ArrowBomb");
        ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowBomb").GetComponent<ParticleSystem>();
        ps.transform.position = hitbox.transform.position;
        ps.Play();

        yield return new WaitForSeconds(0.8f);
        Managers.Resource.Destroy(ps.gameObject);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}

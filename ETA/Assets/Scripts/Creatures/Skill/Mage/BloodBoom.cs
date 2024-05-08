using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBoom : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(5, 5, 5);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/BloodBoom.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("CASTING_IN", 0.1f);
        Managers.Sound.Play("Skill/BloodBoom1");
        HitBox hiddenbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hiddenbox.transform.position = _skillSystem.TargetPosition;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.BloodExplosion, hiddenbox.transform);

        yield return new WaitForSeconds(0.3f);
        _animator.CrossFade("CASTING_WAIT", 0.1f);

        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("Skill/BloodBoom1");

        yield return new WaitForSeconds(0.5f);
        _animator.CrossFade("CASTING_OUT", 0.1f);

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("SKILL2", 0.1f);
        Managers.Sound.Play("Skill/BloodBoom2");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(1.5f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Effect.Stop(ps);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSkill : Skill
{

    protected override void Init()
    {
        SetCoolDownTime(4);
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(7,7,7);
    }
    public override IEnumerator StartSkillCast()
    {

        _animator.CrossFade("COLLAVO", 0.1f);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.5f);
        _controller.ChangeState(_controller.MOVE_STATE);

    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);

        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/WarriorHoldSkill");
        ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/SpikeFieldVolcanic").GetComponent<ParticleSystem>();
        ps1.transform.position = _skillSystem.TargetPosition;
        ps1.Play();
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/WarriorHoldSkill");
        ParticleSystem ps2 = Managers.Resource.Instantiate("Effect/SpikeFieldVolcanic").GetComponent<ParticleSystem>();
        ps2.transform.position = _skillSystem.TargetPosition + gameObject.transform.forward * 3;
        ps2.Play();
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/WarriorHoldSkill");
        ParticleSystem ps3 = Managers.Resource.Instantiate("Effect/SpikeFieldVolcanic").GetComponent<ParticleSystem>();
        ps3.transform.position = _skillSystem.TargetPosition + gameObject.transform.forward * 6;
        ps3.Play();

        yield return new WaitForSeconds(0.6f);
        HitBox hitbox1 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox1.SetUp(transform, Damage);
        hitbox1.transform.position = _skillSystem.TargetPosition;
        hitbox1.transform.localScale = skillRange;

        HitBox hitbox2 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox2.SetUp(transform, Damage);
        hitbox2.transform.position = _skillSystem.TargetPosition + gameObject.transform.forward * 7;
        hitbox2.transform.localScale = skillRange;

        HitBox hitbox3 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox3.SetUp(transform, Damage);
        hitbox3.transform.position = _skillSystem.TargetPosition + gameObject.transform.forward * 14;
        hitbox3.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox1.gameObject);
        Managers.Resource.Destroy(hitbox2.gameObject);
        Managers.Resource.Destroy(hitbox3.gameObject);

        _controller.ChangeState(_controller.MOVE_STATE);

        yield return new WaitForSeconds(1.3f);
        Managers.Sound.Play("Skill/WarriorHoldSkill2");
        Managers.Resource.Destroy(ps1.gameObject);
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/WarriorHoldSkill2");
        Managers.Resource.Destroy(ps2.gameObject);
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/WarriorHoldSkill2");
        Managers.Resource.Destroy(ps3.gameObject);
    }
}

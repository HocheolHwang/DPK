using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCrash : Skill
{
    private Coroutine groundcrashCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/GroundCrash.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        yield return new WaitForSeconds(0.7f);

        groundcrashCoroutine = StartCoroutine(GroundCrashCoroutine());

        yield return new WaitForSeconds(1.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator GroundCrashCoroutine()
    {
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox1 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox1.SetUp(transform, Damage);
        hitbox1.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox1.transform.localScale = new Vector3(3, 3, 3);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.GroundCrash, 1.5f, hitbox1.gameObject.transform);

        yield return new WaitForSeconds(0.6f);
        Managers.Resource.Destroy(hitbox1.gameObject);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox2 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox2.SetUp(transform, Damage);
        hitbox2.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox2.transform.localScale = new Vector3(3, 3, 3);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox2.gameObject);

        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox3 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox3.SetUp(transform, Damage);
        hitbox3.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox3.transform.localScale = new Vector3(2, 2, 2);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox3.gameObject);

        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox4 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox4.SetUp(transform, Damage * 2);
        hitbox4.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox4.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox4.gameObject);
    }
}

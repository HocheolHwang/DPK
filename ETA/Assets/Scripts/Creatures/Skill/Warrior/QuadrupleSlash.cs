using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupleSlash : Skill
{
    private Coroutine drawswordCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(3);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {
        
        
        yield return new WaitForSeconds(0.05f);
        Managers.Sound.Play("Skill/RSkill");
        _animator.CrossFade("ATTACK5", 0.05f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.QuadrupleSlash1, 0.0f, transform);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        if (_controller.SkillSlot.PreviousSkill is TripleSlash)
        {
            Managers.Coroutine.Run(TelekineticSwordsCoroutine());
        }

        ps = Managers.Effect.Play(Define.Effect.QuadrupleSlash2, 0.0f, transform);
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        ps = Managers.Effect.Play(Define.Effect.QuadrupleSlash3, 0.0f, transform);
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        ps = Managers.Effect.Play(Define.Effect.QuadrupleSlash4, 0.0f, transform);
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);




        yield return new WaitForSeconds(0.45f);

        ChangeToPlayerMoveState();
    }

    private IEnumerator TelekineticSwordsCoroutine()
    {
        Managers.Sound.Play("Skill/TargetSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.SwordVolleyBlue, 2.0f, hitbox.transform);

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.20f);
            Managers.Resource.Destroy(hitbox.gameObject);
            hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition;
            Managers.Sound.Play("Skill/TargetSkill");
        }
    }


}

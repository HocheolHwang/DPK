using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleSlash : Skill
{
    private Coroutine drawswordCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(5);
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
        _animator.CrossFade("JUMPATTACK1", 0.05f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.TripleSlash1, 0.0f, transform);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);


        _animator.CrossFade("JUMPATTACK2", 0.05f);
        ps = Managers.Effect.Play(Define.Effect.TripleSlash2, 0.0f, transform);
        Managers.Sound.Play("Skill/RSkill");
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        
        _animator.CrossFade("JUMPATTACK3", 0.05f);
        if (_controller.SkillSlot.PreviousSkill is DoubleSlash)
        {
            Managers.Coroutine.Run(TelekineticSwordsCoroutine());
        }
        yield return new WaitForSeconds(0.4f);
        ps = Managers.Effect.Play(Define.Effect.TripleSlash3, 0.0f, transform);
        Managers.Sound.Play("Skill/RSkill");
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.05f);
        Managers.Resource.Destroy(hitbox.gameObject);



        ChangeToPlayerMoveState();
    }

    private IEnumerator TelekineticSwordsCoroutine()
    {
        Managers.Sound.Play("Skill/Lightning");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.LightningStrikeBlue, 2.0f, hitbox.transform);
        yield return null;

    }


}

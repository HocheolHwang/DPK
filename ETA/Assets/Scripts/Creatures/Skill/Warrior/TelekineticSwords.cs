using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekineticSwords : Skill
{
    private Coroutine telekineticswordsCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(7);
        Damage = 10;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/TelekineticSwords.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);

        telekineticswordsCoroutine = StartCoroutine(TelekineticSwordsCoroutine());

        yield return new WaitForSeconds(1.5f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }



    private IEnumerator TelekineticSwordsCoroutine()
    {
        Managers.Sound.Play("Skill/TargetSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
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

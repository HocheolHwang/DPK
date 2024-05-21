using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBoom : Skill
{
    private Coroutine bloodboomCoroutine;
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 50;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(6, 6, 6);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/BloodBoom.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("CASTING_IN", 0.1f);

        yield return new WaitForSeconds(0.2f);
        if (_controller.Stat.Hp <= 15)
            StartCoroutine(LowHpCoroutine());
        else
        {
            bloodboomCoroutine = StartCoroutine(BloodboomCoroutine());
            _animator.CrossFade("CASTING_WAIT", 0.1f);

            yield return new WaitForSeconds(0.2f);
            _animator.CrossFade("CASTING_OUT", 0.1f);

            yield return new WaitForSeconds(0.2f);
            _animator.CrossFade("SKILL2", 0.1f);
        }


        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator BloodboomCoroutine()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _controller.DecreaseHp(15);
        Managers.Sound.Play("Skill/BloodBoom1");

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.BloodExplosion, 3.0f, transform);
        ps.transform.position = _skillSystem.TargetPosition;

        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("Skill/BloodBoom1");

        yield return new WaitForSeconds(1.0f);
        Managers.Sound.Play("Skill/BloodBoom2");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage * 4);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(1.5f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    private IEnumerator LowHpCoroutine()
    {
        Managers.Sound.Play("Skill/LowHp");
        yield return new WaitForSeconds(0.25f);
    }
}

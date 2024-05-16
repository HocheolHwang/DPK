using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(15);
        Damage = 5;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(1, 5, 5);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/FireWall.png");
    }
    public override IEnumerator StartSkillCast()
    {
        Vector3 rangePos = _skillSystem.TargetPosition;
        _animator.CrossFade("CASTING_IN", 0.1f);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FireWallCoroutine(rangePos));
        _animator.CrossFade("CASTING_WAIT", 0.1f);

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("CASTING_OUT", 0.1f);

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("SKILL2", 0.1f);


        yield return new WaitForSeconds(0.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator FireWallCoroutine(Vector3 rangePos)
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage / 5;
        Managers.Sound.Play("Skill/FireWall");

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.FireWall, 1.5f, transform);
        ps.transform.position = rangePos;
        ps.transform.rotation = Quaternion.Euler(0, 90, 0);

        for (int i = 0; i < 5; i++)
        {
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = rangePos + new Vector3(0, 0, -0.5f);
            hitbox.transform.localScale = new Vector3(6, 5, 1);

            yield return new WaitForSeconds(0.3f);
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
    }
}

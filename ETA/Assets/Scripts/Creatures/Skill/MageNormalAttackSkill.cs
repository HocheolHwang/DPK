using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageNormalAttackSkill : Skill
{
    [Header("개발 편의성")]
    [SerializeField] float _duration = 1.5f;
    [SerializeField] float _speed = 7.0f;
    [SerializeField] float _upLoc = 1.0f;
    protected override void Init()
    {
        _createTime = 0.1f;

        Damage = 20;

        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(1f, 1.0f, 1f);
        base.Init();

    }

    public override IEnumerator StartSkillCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        if (_controller.StateMachine.CurState is PlayerStates.SkillState) yield break;
        _animator.CrossFade("NORMAL_ATTACK", 0.05f);

        yield return new WaitForSeconds(_createTime);

        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/EnergyBall");
        Managers.Coroutine.Run(EnergyBall());

        yield return new WaitForSeconds(_duration+ 0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    IEnumerator EnergyBall()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;

        Vector3 rootForward = transform.TransformDirection(Vector3.forward);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, 1, false, 2.0f);
        hitbox.transform.localScale = skillRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.MageNormalAttackEffect, gameObject.transform);
        ps.transform.rotation = hitbox.transform.rotation;
        ps.transform.position = hitbox.transform.position;
        ps.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        float timer = 0;
        while (timer <= _duration)
        {
            Vector3 moveStep = hitbox.transform.forward * _speed * Time.deltaTime;
            hitbox.transform.position += moveStep;
            ps.transform.position += moveStep;

            timer += Time.deltaTime;

            if (hitbox.Penetration == 0)
            {
                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Resource.Destroy(ps.gameObject);

                // hit event를 여기서 실행시키면 됨

                yield break;
            }

            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(1.3f);

        Managers.Effect.Stop(ps);
        ChangeToPlayerMoveState();
    }
}

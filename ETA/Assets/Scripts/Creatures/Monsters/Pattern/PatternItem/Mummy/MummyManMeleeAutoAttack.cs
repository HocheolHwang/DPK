using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KnightGCounterAttackPattern - hitbox 참고
// KnightGTwoSkillAttack - 여러 hitbox 만들기
// wind mill effect만 존재( 0 )
// hit box 3개
// sound 2개( first, second | wind mill )
public class MummyManMeleeAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.3f;
    [SerializeField] float _hitboxRadius = 6.0f;
    [SerializeField] float _upLoc = 1.0f;
    [SerializeField] float _rightLoc = 0f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        _patternDmg = 130;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc));
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp + rootRight;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.KnightG_CounterAttack, _controller.transform);
        ps.GetComponent<AudioSource>().Play();
        ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(ps.main.duration);
        Managers.Resource.Destroy(ps.gameObject);
    }
}

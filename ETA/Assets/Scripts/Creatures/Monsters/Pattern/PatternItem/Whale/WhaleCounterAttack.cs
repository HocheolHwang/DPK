using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleCounterAttackPattern : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.3f;
    [SerializeField] float _hitboxRadius = 6.0f;
    [SerializeField] float _upLoc = 1.0f;
    [SerializeField] float _rightLoc = 0f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(7f, 7f, 7f);

    public override void Init()
    {
        base.Init();

        _createTime = 0.7f;
        _patternRange = _hitboxRange;
        _patternDmg = 5;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc));
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp + rootRight;

        yield return new WaitForSeconds(_createTime + 0.2f);
        Managers.Sound.Play("Monster/Whale/counter_wave", Define.Sound.Effect);
        yield return new WaitForSeconds(0.4f);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WaterSplashSoft, _controller.transform);
        ps.transform.position = transform.position;


        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = transform.position;


        yield return new WaitForSeconds(1.3f);
        Managers.Sound.Play("Monster/Whale/healpop-46004", Define.Sound.Effect);
        Managers.Resource.Destroy(ps.gameObject);

    }
}

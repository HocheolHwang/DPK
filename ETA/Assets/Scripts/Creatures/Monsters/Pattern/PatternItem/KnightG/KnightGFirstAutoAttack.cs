using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGFirstAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.3f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(3.5f, 3.0f, 3.5f);
    [SerializeField] float _upLoc = 1.0f;
    [SerializeField] float _rightLoc = 0f;

    public override void Init()
    {
        base.Init();

        _createTime = 1.2f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc));
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp + rootRight;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //ParticleSystem ps = Managers.Resource.Instantiate($"Effect/{_effectName}").GetComponent<ParticleSystem>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Resource.Destroy(ps.gameObject);
    }
}

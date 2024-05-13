using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisPatternTwoWindMill : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 7.2f;
    [SerializeField] float _upPos = 1.5f;
    [SerializeField] float _interval = 0.3f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternDmg = 50;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = _controller.transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_PatternTwoWindMill, transform);
        ps.transform.position = Pos;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.transform.position = Pos;
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;

        yield return new WaitForSeconds(_interval);
        hitbox.SetActiveCollider();

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}

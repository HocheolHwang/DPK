using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IprisPatternTwo : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 7.0f;
    [SerializeField] float _upPos = 1.0f;
    [SerializeField] float _forwardPos = 1.3f;

    private const float TIME = 3.0F;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternDmg = 80;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 destToTarget = MonsterManager.Instance.GetBackPosPlayer(_controller.transform);
        yield return new WaitForSeconds(TIME * 0.4f);

        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 Pos = _controller.transform.position + rootUp + rootForward;

        yield return StartCoroutine(DownAttack(Pos));
    }

    IEnumerator DownAttack(Vector3 Pos)
    {
        // 내려찍기 수행
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.transform.position = Pos;
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;

        // EFFECT
        ParticleSystem rushingPS = Managers.Effect.Play(Define.Effect.Ipris_PatternTwo, 0, transform);
        rushingPS.transform.position = Pos;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class KnightGPhaseAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 2.5f;
    [SerializeField] float _upLoc = 1.0f;
    [SerializeField] float _expandSpeed = 10.5f;
    [SerializeField] float _duration = 0.4f;

    //Coroutine co;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        // 
        // _patternDmg = 
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _controller.Detector.AttackRange);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        Managers.Coroutine.Run(expandCollider(objectLoc));

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.KnightG_PhaseAttack, 0, _controller.transform);
        Managers.Sound.Play("Monster/KnightG/KnightGPhaseAttack_SND", Define.Sound.Effect);
        ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);


    }

    private IEnumerator expandCollider(Vector3 loc)
    {
        HitBox spreadHitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        spreadHitbox.SetUp(transform, _attackDamage - 10, -1, false, _duration);
        spreadHitbox.transform.rotation = transform.rotation;
        spreadHitbox.transform.position = loc;

        SphereCollider spreadCollider = spreadHitbox.GetComponent<SphereCollider>();
        spreadCollider.radius = 2.0f;

        float timer = 0;
        while (timer <= _duration)
        {
            spreadCollider.radius += _expandSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(spreadHitbox.gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boar의 근거리 공격
public class ButterflyAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _duration = 1.5f;
    [SerializeField] float _speed = 14.0f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] float _upLoc = 1.0f;

    private int penetration = 1;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, penetration, false, _duration);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SparkleMissileGreen, _controller.transform);
        ps.transform.rotation = hitbox.transform.rotation;
        ps.transform.position = hitbox.transform.position;


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
                Managers.Effect.Stop(ps);

                // hit event를 여기서 실행시키면 됨

                yield break;
            }

            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Effect.Stop(ps);
    }
}

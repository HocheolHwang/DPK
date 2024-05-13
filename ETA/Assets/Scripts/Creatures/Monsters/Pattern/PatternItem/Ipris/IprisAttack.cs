using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class IprisAttack : Pattern
{
    [Header("First Attack")]
    [SerializeField] float _forwardPos = 1.0f;
    [SerializeField] float _upPos = 1.5f;
    [SerializeField] float _duration = 2.0f;
    [SerializeField] float _speed = 15.0f;
    [SerializeField] float _hitboxRadius = 1.0f;
    private int penetration = 1;


    private IprisAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 0.4f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        yield return new WaitForSeconds(_createTime);   // throw the Fireball

        StartCoroutine(FirstAttack(Pos));

        yield return new WaitForSeconds(_animData.AttackFirstAnim.length - _createTime);


        yield return new WaitForSeconds(_animData.AttackSecondAnim.length);
    }

    IEnumerator FirstAttack(Vector3 Pos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, penetration, false, _duration);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = _controller.transform.rotation;
        hitbox.transform.position = Pos;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_AttackFirst, _duration);
        ps.transform.position = Pos;
        ps.transform.rotation = _controller.transform.rotation;

        float timer = 0;
        Vector3 dir = DirectionToTarget(ps.transform.position);
        while (timer <= _duration)
        {
            Vector3 moveStep = dir * _speed * Time.deltaTime;

            Vector3 hitboxNewPos = hitbox.transform.position + moveStep;
            hitboxNewPos.y = hitbox.transform.position.y;
            hitbox.transform.position = hitboxNewPos;

            Vector3 psNewPos = ps.transform.position + moveStep;
            psNewPos.y = ps.transform.position.y;
            ps.transform.position = psNewPos;

            timer += Time.deltaTime;

            if (hitbox.Penetration == 0)
            {
                // hit event를 여기서 실행시키면 됨
                // Stop Action -> Destroy
                ParticleSystem hitPs = Managers.Effect.Play(Define.Effect.Ipris_CounterAttackExplo, 0);
                hitPs.transform.position = ps.transform.position;

                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Effect.Stop(ps);
                yield break;
            }

            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}

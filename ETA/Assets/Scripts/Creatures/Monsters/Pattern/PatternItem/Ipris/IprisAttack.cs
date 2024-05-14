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

    [Header("Second Attack")]
    [SerializeField] float _upPos2 = 1.5f;
    [SerializeField] float _speed2 = 30.0f;
    [SerializeField] float _duration2 = 1.0f;
    [SerializeField] Vector3 _hitboxSize = new Vector3(1.0f, 3.0f, 1.0f);


    private IprisAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _animData = _controller.GetComponent<IprisAnimationData>();
        _createTime = 0.4f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        Vector3 rootUp2 = transform.TransformDirection(Vector3.up * _upPos2);
        Vector3 Pos2 = transform.position + rootUp2;

        yield return new WaitForSeconds(_createTime);   // throw the Fireball

        StartCoroutine(FirstAttack(Pos, _duration, _speed));

        yield return new WaitForSeconds(_animData.AttackFirstAnim.length - _createTime);

        StartCoroutine(SecondAttack(Pos2, _duration2, _speed2));

        yield return new WaitForSeconds(_animData.AttackSecondAnim.length + _createTime * 2);
        // coroutine 유지를 위한 잠깐의 시간
    }

    IEnumerator FirstAttack(Vector3 Pos, float duration, float speed)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, penetration, false, duration);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = _controller.transform.rotation;
        hitbox.transform.position = Pos;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_AttackFirst, duration);
        ps.transform.position = Pos;
        ps.transform.rotation = _controller.transform.rotation;

        float timer = 0;
        Vector3 dir = DirectionToTarget(ps.transform.position);
        while (timer <= duration)
        {
            Vector3 moveStep = dir * speed * Time.deltaTime;

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

    IEnumerator SecondAttack(Vector3 Pos, float duration, float speed)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_AttackSecond, duration);
        ps.transform.position = Pos;
        ps.transform.rotation = _controller.transform.rotation;

        yield return new WaitForSeconds(0.2f);

        // 넉백 히트박스 추가

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, -1, false, duration);
        hitbox.transform.localScale = _hitboxSize;
        hitbox.transform.rotation = _controller.transform.rotation;
        hitbox.transform.position = Pos;

        Managers.Sound.Play("Sounds/Monster/Ipris/IprisAttackSecond_SND", Define.Sound.Effect);

        float timer = 0;
        Vector3 dir = DirectionToTarget(_controller.transform.position);
        while (timer <= duration)
        {
            Vector3 moveStep = dir * speed * Time.deltaTime;

            Vector3 hitboxNewPos = hitbox.transform.position + moveStep;
            hitboxNewPos.y = hitbox.transform.position.y;
            hitbox.transform.position = hitboxNewPos;

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}

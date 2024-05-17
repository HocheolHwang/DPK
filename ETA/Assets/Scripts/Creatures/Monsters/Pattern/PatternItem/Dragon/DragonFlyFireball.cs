using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlyFireball : Pattern
{
    [Header("effect options")]
    //[SerializeField] Vector3 _mousePos = new Vector3(0, 0.7f, 2.0f);               // 드래곤이 날았을 때의 높이
    [SerializeField] float _upPos = 12.0f;
    [SerializeField] float _forwardPos = 3.0f;
    [SerializeField] float _fireballSpeed = 10.0f;

    [Header("hit box options")]
    [SerializeField] float _fireRadius = 12.0f;
    [SerializeField] float _fieldRadius = 8.0f;
    [SerializeField] int _exploDMG = 250;
    [SerializeField] int _fieldDMG = 25;

    public override void Init()
    {
        base.Init();

        _createTime = 0.2f;
    }

    public override IEnumerator StartPatternCast()
    {
        // 파이어볼 위치와 타겟 위치
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 flyPos = transform.position + rootUp + rootForward;
        Vector3 targetPos = MonsterManager.Instance.GetCenterPos(transform);

        yield return new WaitForSeconds(_createTime);

        // 파이어볼 생성 및 발사
        yield return StartCoroutine(Fireball(flyPos, targetPos));

        yield return StartCoroutine(Explosion(targetPos));

        yield return new WaitForSeconds(0.5f);

        // 소리 시간만큼 화염 지대가 지속
        yield return FireField(targetPos, 3.5f);


        yield return new WaitForSeconds(5.0f);
    }

    IEnumerator Fireball(Vector3 flyPos, Vector3 targetPos)
    {
        float duration = Vector3.Distance(flyPos, targetPos) / _fireballSpeed;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_Fireball, duration, transform);
        ps.transform.position = flyPos;

        float timer = 0;
        Vector3 dir = (targetPos - flyPos).normalized;
        while (timer <= duration)
        {
            Vector3 moveStep = dir * _fireballSpeed * Time.deltaTime;
            ps.transform.position += moveStep;

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Effect.Stop(ps);
    }

    IEnumerator Explosion(Vector3 targetPos)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_FireballExplo, 5.0f);
        ps.transform.position = targetPos;
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonFireballExplo_SND", Define.Sound.Effect);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.gameObject.name = "Fire Ball hitbox";
        hitbox.SetUp(transform, _exploDMG);
        hitbox.GetComponent<SphereCollider>().radius = _fireRadius;
        hitbox.transform.position = targetPos;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    IEnumerator FireField(Vector3 targetPos, float duration)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_FireballField, duration, transform);
        ps.transform.position = targetPos;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.gameObject.name = "Fire Field hitbox";
        hitbox.SetUp(transform, _fieldDMG);
        hitbox.GetComponent<SphereCollider>().radius = _fieldRadius;
        hitbox.transform.position = targetPos;

        yield return new WaitForSeconds(1.0f);
        hitbox.SetActiveColliderLimit(0.1f);

        yield return new WaitForSeconds(1.0f);
        hitbox.SetActiveColliderLimit(0.1f);

        yield return new WaitForSeconds(1.0f);
        hitbox.SetActiveColliderLimit(0.1f);

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(duration - 3.15f);
        Managers.Effect.Stop(ps);
    }
}

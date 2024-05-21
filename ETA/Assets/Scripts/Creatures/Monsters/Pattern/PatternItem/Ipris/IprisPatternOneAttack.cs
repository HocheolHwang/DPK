using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisPatternOneAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _spreadInitRadius = 2.0f;
    [SerializeField] float _upPos = 1.5f;
    [SerializeField] float _expandSpeed = 40f;
    [SerializeField] float _duration = 0.4f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 objectPos = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        Managers.Coroutine.Run(expandCollider(objectPos));

        yield return new WaitForSeconds(_duration);
    }

    private IEnumerator expandCollider(Vector3 Pos)
    {
        HitBox spreadHitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        spreadHitbox.SetUp(transform, _attackDamage, -1, false, _duration);
        spreadHitbox.transform.rotation = transform.rotation;
        spreadHitbox.transform.position = Pos;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_PatternOneAttack, 0, null);
        ps.transform.position = _controller.transform.position;

        SphereCollider spreadCollider = spreadHitbox.GetComponent<SphereCollider>();
        spreadCollider.radius = _spreadInitRadius;

        Managers.Sound.Play("Sounds/Monster/Ipris/IprisPatternOne_SND", Define.Sound.Effect);

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

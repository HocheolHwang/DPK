using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PorinAutoAttack : Pattern
{
    [Header("원하는 이펙트 이름을 넣으세요 - 디버깅")]
    [SerializeField] string _effectName;

    [Header("개발 편의성")]
    [SerializeField] float _duration = 1.5f;
    [SerializeField] float _speed = 14.0f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] float _upLoc = 1.0f;

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
        //ParticleSystem ps = Managers.Resource.Instantiate($"Effect/{_effectName}").GetComponent<ParticleSystem>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        float timer = 0;
        while (timer < _duration)
        {
            Vector3 moveStep = hitbox.transform.forward * _speed * Time.deltaTime;
            hitbox.transform.position += moveStep;

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Resource.Destroy(ps.gameObject);
    }
}

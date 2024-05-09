using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFishAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.7f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(0.9f, 0.5f, 1.3f);
    [SerializeField] float _upLoc = 0.4f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.3f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        Managers.Sound.Play("Monster/StarFish/twinklesparkle", Define.Sound.Effect);
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc)); // Target이 null일 수 있기 때문에 임의로 지정
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp;
        //Vector3 dir = _controller.Detector.Target.position - transform.position;
        //Vector3 _objectPosition = transform.position + dir.normalized * _controller.Detector.AttackRange;

        yield return new WaitForSeconds(_createTime);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //ParticleSystem ps = Managers.Resource.Instantiate($"Effect/{_effectName}").GetComponent<ParticleSystem>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        //ps.transform.position = hitbox.transform.position;



        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Resource.Destroy(ps.gameObject);
    }
}

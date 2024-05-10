using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerDryadCounterAttackPattern : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc = 0.3f;
    [SerializeField] float _hitboxRadius = 6.0f;
    [SerializeField] float _upLoc = 1.0f;
    [SerializeField] float _rightLoc = 0f;

    public override void Init()
    {
        base.Init();

        _createTime = 1.7f;
        _patternDmg = 130;
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange - _hitboxForwardLoc));
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightLoc);
        Vector3 objectLoc = transform.position + rootForward + rootUp + rootRight;

        yield return new WaitForSeconds(_createTime);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.FlowerDryad_CounterAttack, _controller.transform);
        ps.transform.position = transform.position;


        yield return new WaitForSeconds(1.3f);
        Managers.Sound.Play("Monster/FlowerDryad/healpop-46004", Define.Sound.Effect);
        gameObject.GetComponent<BaseController>().IncreaseHp(100);
        Managers.Resource.Destroy(ps.gameObject);

    }
}

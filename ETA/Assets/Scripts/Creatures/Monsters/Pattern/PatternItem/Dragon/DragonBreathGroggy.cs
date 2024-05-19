using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathGroggy : Pattern
{
    [Header("effect options")]
    [SerializeField] float _duration;
    [SerializeField] float _upPos = 2.0f;
    [SerializeField] Vector3 _localPos = new Vector3(0, 3.0f, 3.5f);

    private DragonAnimationData _animData;
    private DragonController _dcontroller;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _duration = _animData.BreathEnableAnim.length;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootUp;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Groggy, _animData.GroggyAnim.length, _controller.transform);
        ps.transform.SetParent(_dcontroller.transform);
        ps.transform.localPosition = _localPos;

        ParticleSystem hitPS = Managers.Effect.Play(Define.Effect.Dragon_BreathGroggy, _animData.GroggyAnim.length, _controller.transform);
        hitPS.transform.position = Pos;

        int AmountDMG = (_dcontroller.Stat.MaxHp / 10) + _dcontroller.Stat.Defense;
        _dcontroller.TakeDamage(AmountDMG);
        
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonGroggy_SND", Define.Sound.Effect);
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonGroggyHit_SND", Define.Sound.Effect);

        yield return new WaitForSeconds(_duration);
    }
}

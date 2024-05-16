using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathEnable : Pattern
{
    [Header("effect options")]
    [SerializeField] float _upPos = 2.0f;
    [SerializeField] float _backPos = 3.0f;
    [SerializeField] Vector3 _scale = new Vector3(5.0f, 8.0f, 5.0f);
    [SerializeField] float _duration;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _duration = _animData.BreathEnableAnim.length * 3.0f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 rootBack = transform.TransformDirection(Vector3.back * _backPos);
        Vector3 auraPos = transform.position + rootUp + rootBack;

        StartCoroutine(BreathEffect(auraPos));

        yield return new WaitForSeconds(_duration);

    }

    IEnumerator BreathEffect(Vector3 Pos)
    {
        _dcontroller.BreathReadyEffect.Play();

        ParticleSystem auraPS = Managers.Effect.ContinuePlay(Define.Effect.Dragon_BreathEnableHit, _controller.transform);
        auraPS.transform.position = Pos;
        auraPS.transform.localScale = _scale;

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonBreathEnableLong_SND", Define.Sound.Effect);

        float timer = 0;
        while (timer <= _duration)
        {
            if (_dcontroller.IsMeetConditionHit)
            {
                Managers.Effect.Stop(auraPS);
                _dcontroller.BreathReadyEffect.Stop();
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Effect.Stop(auraPS);
        _dcontroller.BreathReadyEffect.Stop();
    }
}

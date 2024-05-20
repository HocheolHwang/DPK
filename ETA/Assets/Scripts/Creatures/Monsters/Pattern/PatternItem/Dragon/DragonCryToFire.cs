using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DragonCryToFire : Pattern
{
    [Header("ps options")]
    [SerializeField] float _leftPos = 3.5f;
    [SerializeField] float _rightPos = 3.5f;
    [SerializeField] float _forwardPos = 1.5f;
    [SerializeField] float _upPos = 2.0f;
    [SerializeField] float _cryTime;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;
    private float _wingDuration;

    public override void Init()
    {
        base.Init();
        _createTime = 0.1f;
        _cryTime = 0.9f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _wingDuration = _animData.CryToFireAnim.length;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootLeft = transform.TransformDirection(Vector3.left * _leftPos);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightPos);
        Vector3 counterWingLeftPos = transform.position + rootUp + rootLeft;
        Vector3 counterWingRightPos = transform.position + rootUp + rootRight;
        Vector3 counterBodyPos = transform.position + rootUp + rootForward;

        yield return new WaitForSeconds(_createTime);

        Managers.Sound.Play("Sounds/Monster/CounterEnergyRed2_SND", Define.Sound.Effect);

        StartCoroutine(CreateWingEffect(counterWingLeftPos, counterWingRightPos, counterBodyPos, _wingDuration));

        yield return new WaitForSeconds(_cryTime);

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonCryToFire_SND", Define.Sound.Effect);

        yield return new WaitForSeconds(1.0f);

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonCryToFire_SND", Define.Sound.Effect);

        yield return new WaitForSeconds(5.0f);
    }

    IEnumerator CreateWingEffect(Vector3 leftPos, Vector3 rightPos, Vector3 bodyPos, float duration)
    {
        ParticleSystem leftPS = Managers.Effect.ContinuePlay(Define.Effect.CounterEnable, _controller.transform);
        ParticleSystem rightPS = Managers.Effect.ContinuePlay(Define.Effect.CounterEnable, _controller.transform);
        leftPS.transform.position = leftPos;
        rightPS.transform.position = rightPos;


        ParticleSystem.MainModule leftPSMainModule = leftPS.main;
        ParticleSystem.MainModule rightPSMainModule = rightPS.main;
        leftPSMainModule.startLifetime = duration;
        rightPSMainModule.startLifetime = duration;

        float timer = 0;
        while (timer <= duration)
        {
            if (_dcontroller.IsMeetConditionDown)
            {
                Managers.Effect.Stop(leftPS);
                Managers.Effect.Stop(rightPS);
                Managers.Sound.Play("Sounds/Monster/CounterEnable_SND", Define.Sound.Effect);

                ParticleSystem counterEffectLeft = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1);
                counterEffectLeft.transform.position = leftPos;
                counterEffectLeft.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

                ParticleSystem counterEffectRight = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1);
                counterEffectRight.transform.position = rightPos;
                counterEffectRight.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

                yield return StartCoroutine(CreateBodyEffect(bodyPos, duration - timer));
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Effect.Stop(leftPS);
        Managers.Effect.Stop(rightPS);
    }

    IEnumerator CreateBodyEffect(Vector3 Pos, float duration)
    {
        ParticleSystem ps = Managers.Effect.ContinuePlay(Define.Effect.CounterEnable, _controller.transform);
        ps.transform.position = Pos;

        ParticleSystem.MainModule psMainModule = ps.main;
        psMainModule.startLifetime = duration;

        float timer = 0;
        while (timer <= duration)
        {
            if (_dcontroller.IsMeetConditionFire)
            {
                Managers.Effect.Stop(ps);
                Managers.Sound.Play("Sounds/Monster/CounterEnable_SND", Define.Sound.Effect);

                ParticleSystem counterEffect = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1);
                counterEffect.transform.position = Pos;
                counterEffect.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

                yield return new WaitForSeconds(1.0f);
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Effect.Stop(ps);
    }
}

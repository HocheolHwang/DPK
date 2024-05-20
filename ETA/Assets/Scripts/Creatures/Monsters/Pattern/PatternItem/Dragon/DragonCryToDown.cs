using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCryToDown : Pattern
{
    [Header("ps options")]
    [SerializeField] float _leftPos = 3.5f;
    [SerializeField] float _rightPos = 3.5f;
    [SerializeField] float _upPos = 2.0f;
    [SerializeField] float _cryTime;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;
    private float _duration;

    public override void Init()
    {
        base.Init();
        _createTime = 0.1f;
        _cryTime = 0.7f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _duration = _animData.FearEnableAnim.length * 1.34f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 rootLeft = transform.TransformDirection(Vector3.left * _leftPos);
        Vector3 rootRight = transform.TransformDirection(Vector3.right * _rightPos);
        Vector3 counterWingLeftPos = transform.position + rootUp + rootLeft;
        Vector3 counterWingRightPos = transform.position + rootUp + rootRight;

        yield return new WaitForSeconds(_createTime);

        Managers.Sound.Play("Sounds/Monster/CounterEnergyRed2_SND", Define.Sound.Effect);
        
        StartCoroutine(CreateBox(counterWingLeftPos));
        StartCoroutine(CreateBox(counterWingRightPos));

        yield return new WaitForSeconds(_cryTime);

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonCryToDown_SND", Define.Sound.Effect);

        yield return new WaitForSeconds(_duration + 3.0f);
    }

    IEnumerator CreateBox(Vector3 Pos)
    {
        ParticleSystem ps = Managers.Effect.ContinuePlay(Define.Effect.CounterEnable, _controller.transform);
        ps.transform.position = Pos;
        ParticleSystem.MainModule psMainModule = ps.main;
        psMainModule.startLifetime = _duration;

        float timer = 0;
        while (timer <= _duration)
        {
            if (_dcontroller.IsMeetConditionDown)
            {
                Managers.Effect.Stop(ps);
                Managers.Sound.Play("Sounds/Monster/CounterEnable_SND", Define.Sound.Effect);

                ParticleSystem counterEffect = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1, transform);
                counterEffect.transform.parent = null;
                counterEffect.transform.position = Pos;
                counterEffect.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

                yield return new WaitForSeconds(3.0f);

                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Effect.Stop(ps);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFearEnable : Pattern
{
    [Header("box options")]
    [SerializeField] float _forwardPos = 1.5f;
    [SerializeField] float _upPos = 2.0f;
    [SerializeField] Vector3 _hitboxRange;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;
    private float _duration;
    private int _penetration = 1;

    public override void Init()
    {
        base.Init();
        _createTime = 0.1f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _duration = _animData.FearEnableAnim.length * 3.0f;
        _hitboxRange = _controller.GetComponent<BoxCollider>().size;    // (8, 5, 4)
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 forward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 hitboxPos = transform.position + rootUp + forward;
        Vector3 effectPos = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        StartCoroutine(CreateHitbox(hitboxPos, effectPos));
        _dcontroller.FearEnableEffect.Play();

        yield return new WaitForSeconds(_duration);
    }

    IEnumerator CreateHitbox(Vector3 hitboxPos, Vector3 effectPos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, _penetration, false, _duration);
        hitbox.transform.localScale = _hitboxRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = hitboxPos;


        ParticleSystem ps = Managers.Effect.ContinuePlay(Define.Effect.CounterEnable_Red, _controller.transform);
        ps.transform.position = effectPos;
        ParticleSystem.MainModule psMainModule = ps.main;
        psMainModule.startLifetime = _duration;

        // 카운터 도중에 내는 소리
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonFearEnable_SND", Define.Sound.Effect);
        Managers.Sound.Play("Sounds/Monster/CounterEnergyRed2_SND", Define.Sound.Effect);

        float timer = 0;
        while (timer <= _duration)
        {
            if (_controller.IsHitCounter)
            {
                ParticleSystem hitRedPs = Managers.Effect.Play(Define.Effect.CounteredEffect_Red, 1, _controller.transform);
                hitRedPs.transform.SetParent(_controller.transform);
                hitRedPs.transform.localPosition = new Vector3(0, _upPos, 0);

                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Sound.Play("Sounds/Monster/CounterEnableRed_SND", Define.Sound.Effect);

                Managers.Effect.Stop(ps);
                _dcontroller.FearEnableEffect.Stop();
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Effect.Stop(ps);
    }
}

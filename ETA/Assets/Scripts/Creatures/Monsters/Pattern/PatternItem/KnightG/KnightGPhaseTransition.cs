using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 특정 상태 이상에 맞으면 phase 진입을 끊을 수 있다.
public class KnightGPhaseTransition : Pattern
{
    KnightGAnimationData _animData;
    KnightGController _kcontroller;

    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 4.0f, 2.0f);
    [SerializeField] float _upLoc = 2.0f;

    private float _duration;
    private int _penetration = 1;


    public override void Init()
    {
        base.Init();

        _animData = _controller.GetComponent<KnightGAnimationData>();
        _kcontroller = _controller.GetComponent<KnightGController>();
        _duration = _animData.PhaseTransitionAnim.length * 2.0f;   // 2.0f는 StateItem에서 설정한 애니메이션 재생 시간에 영향을 미친다.

        _createTime = 0.1f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        _hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        _hitbox.SetUp(transform, _attackDamage, _penetration, false, _duration);
        _hitbox.transform.localScale = _patternRange;
        _hitbox.transform.rotation = transform.rotation;
        _hitbox.transform.position = objectLoc;

        _ps = Managers.Effect.Play(Define.Effect.KnightG_PhaseTransition, _controller.transform);
        _ps.transform.SetParent(transform);

        Invoke("PlayBeatSound", 0.25f);
        Invoke("PlayBeatSound", 0.55f);
        Invoke("PlayCrySound", 0.7f);

        // 시전 도중에 특정 상태이상 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
        // 상태에게 알려줘야함 -> groggy로 이동
        float timer = 0;
        while (timer < _duration)
        {
            //if (_kcontroller.IsHitCounter)
            //{
            //    Managers.Resource.Destroy(_hitbox.gameObject);
            //    Managers.Effect.Stop(_ps);
            //    // 소리 발생

            //    yield break;
            //}

            //timer += Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(_hitbox.gameObject);
    }

    public void PlayBeatSound()
    {
        Managers.Sound.Play("Monster/KnightG/KnightGPhaseTransitionF_SND", Define.Sound.Effect);
    }

    public void PlayCrySound()
    {
        Managers.Sound.Play("Monster/KnightG/KnightGPhaseTransitionB_SND", Define.Sound.Effect);
    }
}

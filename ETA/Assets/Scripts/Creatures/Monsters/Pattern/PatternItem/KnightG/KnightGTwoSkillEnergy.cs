using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hit box를 통해서 상태 이상 스킬인지 확인 -> counter랑 같은 로직                -> 후순위
// 상태 이상 스킬인 경우 -> controller가 two skill attack 상태일 때 공격력을 낮춤 -> 후순위
public class KnightGTwoSkillEnergy : Pattern
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
        _duration = 2.0f;   // 2.0f는 StateItem에서 설정한 2초 지속시간

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

        _ps = Managers.Effect.Play(Define.Effect.KnightG_TwoSkillEnergy, 0, _controller.transform);
        _ps.transform.position = _hitbox.transform.position;

        // KnightGTwoSkillEnergy_SND
        _ps.GetComponent<AudioSource>().Play();

        // 시전 도중에 특정 상태이상 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
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

            timer += Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(_hitbox.gameObject);
        
    }
}

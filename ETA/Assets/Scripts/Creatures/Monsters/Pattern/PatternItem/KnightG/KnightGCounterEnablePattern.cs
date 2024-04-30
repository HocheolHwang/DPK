using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity;
using UnityEngine;

public class KnightGCounterEnablePattern : Pattern
{
    KnightGAnimationData _animData;
    KnightGController _kcontroller;

    [Header("원하는 이펙트와 HIT_COUNTER 소리 이름을 넣으세요 - 디버깅")]
    [SerializeField] string _soundName;
    

    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 4.0f, 2.0f);
    [SerializeField] float _upLoc = 2.0f;
    //[SerializeField] Color _counterColor;
        //64828C;

    private float _duration;
    //private Renderer[] _allRenderers; // 캐릭터의 모든 Renderer 컴포넌트
    //private Color[] _originalColors;  // 원래의 머티리얼 색상 저장용 배열


    public override void Init()
    {
        base.Init();
        _animData = _controller.GetComponent<KnightGAnimationData>();
        _kcontroller = _controller.GetComponent<KnightGController>();
        _duration = _animData.CounterEnableAnim.length * 4.0f - _createTime;

        _createTime = 0.1f;
        _patternRange = _hitboxRange;

        //SaveOriginColor();
    }

    public override IEnumerator StartPatternCast()
    {
        // 멈췄을 때 target을 향해 hitbox, effect 생성
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        //SetCounterColor();

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.CounterEnable, _controller.transform);
        ps.transform.position = hitbox.transform.position;
        ParticleSystem.MainModule psMainModule = ps.main;
        psMainModule.startLifetime = _animData.CounterEnableAnim.length * 4.0f;

        // 시전 도중에 카운터 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
        float timer = 0;
        while (timer < _duration)
        {
            if (_kcontroller.IsHitCounter)
            {
                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Effect.Stop(ps);
                // 소리 발생

                //RevertToOriginColor();

                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        //RevertToOriginColor();

        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Effect.Stop(ps);
    }

    //public void SaveOriginColor()
    //{
    //    _allRenderers = GetComponentsInChildren<Renderer>();
    //    _originalColors = new Color[_allRenderers.Length];

    //    // 각 Renderer의 원래 머티리얼 색상 저장
    //    for (int i = 0; i < _allRenderers.Length; i++)
    //    {
    //        _originalColors[i] = _allRenderers[i].material.color;
    //    }
    //}

    //public void SetCounterColor()
    //{
    //    bool temp = ColorUtility.TryParseHtmlString("#64828C", out _counterColor);
    //    foreach (Renderer renderer in _allRenderers)
    //    {
    //        renderer.material.SetColor("_Color", _counterColor);
    //        renderer.material.SetColor("_BaseColor", _counterColor);
    //    }
    //}

    //public void RevertToOriginColor()
    //{
    //    // 모든 Renderer의 머티리얼 색상을 원래 색상으로 복구
    //    for (int i = 0; i < _allRenderers.Length; i++)
    //    {
    //        Renderer renderer = _allRenderers[i];
    //        renderer.material.SetColor("_BaseColor", Color.white);
    //        renderer.material.color = _originalColors[i];
    //    }
    //}
}

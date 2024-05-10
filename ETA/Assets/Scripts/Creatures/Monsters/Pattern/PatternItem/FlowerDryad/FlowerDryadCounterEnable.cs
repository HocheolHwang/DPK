using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity;
using UnityEngine;

public class FlowerDryadCounterEnable : Pattern
{
    FlowerDryadAnimationData _animData;
    FlowerDryadController _kcontroller;
    
    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 4.0f, 2.0f);
    [SerializeField] float _upLoc = 2.0f;
    //[SerializeField] Color _counterColor;
        //64828C;

    private float _duration;
    private int _penetration = 1;
    //private Renderer[] _allRenderers; // 캐릭터의 모든 Renderer 컴포넌트
    //private Color[] _originalColors;  // 원래의 머티리얼 색상 저장용 배열


    public override void Init()
    {
        base.Init();
        _animData = _controller.GetComponent<FlowerDryadAnimationData>();
        _kcontroller = _controller.GetComponent<FlowerDryadController>();
        _duration = _animData.CounterEnableAnim.length * 4.0f;        // 4.0f는 StateItem에서 설정한 애니메이션 재생 시간에 영향을 미친다.

        _createTime = 0.1f;
        _patternRange = _hitboxRange;

        //SaveOriginColor();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        //SetCounterColor();

        _hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        _hitbox.SetUp(transform, _attackDamage, _penetration, false, _duration);
        _hitbox.transform.localScale = _patternRange;
        _hitbox.transform.rotation = transform.rotation;
        _hitbox.transform.position = objectLoc;

        
        _ps = Managers.Effect.Play(Define.Effect.CounterEnable, _controller.transform);
        _ps.transform.position = _hitbox.transform.position;
        ParticleSystem.MainModule _psMainModule = _ps.main;
        _psMainModule.startLifetime = _animData.CounterEnableAnim.length * 4.0f;

        Managers.Sound.Play("Monster/KnightG/KnightGCounterEnergy_SND", Define.Sound.Effect);

        // 시전 도중에 카운터 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
        float timer = 0;
        while (timer < _duration)
        {
            if (_kcontroller.IsHitCounter)
            {
                Managers.Resource.Destroy(_hitbox.gameObject);
                Managers.Effect.Stop(_ps);
                Managers.Sound.Play("Monster/CounterEnable_SND", Define.Sound.Effect);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        //RevertToOriginColor();

        Managers.Resource.Destroy(_hitbox.gameObject);
        Managers.Effect.Stop(_ps);
    }

    #region Modify Color
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
    #endregion
}

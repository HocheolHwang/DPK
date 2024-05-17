using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DragonBreath : Pattern
{
    [Header("options")]
    [SerializeField] Vector3 _pos = new Vector3(0.0f, 2.5f, 1.5f);
    [SerializeField] Vector3 _scale = new Vector3(12.0f, 5.0f, 1.0f);
    [SerializeField] float _duration;
    [SerializeField] float _boxDuration = 1.5f;
    [SerializeField] float _speed = 20.0f;
    [SerializeField] float _interval = 0.3f;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _animData = _controller.GetComponent<DragonAnimationData>();
        _dcontroller = _controller.GetComponent<DragonController>();
        _duration = _animData.BreathAnim.length * 2.0f;

        _patternDmg = 10;
    }

    public override IEnumerator StartPatternCast()
    {
        yield return new WaitForSeconds(_createTime);

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonBreathRoar_SND", Define.Sound.Effect);
        _dcontroller.BreathEffect.Play();

        StartCoroutine(Stun(_boxDuration));

        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonBreath_SND", Define.Sound.Effect);

        #region Breath 12번
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        yield return new WaitForSeconds(_interval);
        StartCoroutine(Breath(_boxDuration));
        #endregion

        yield return new WaitForSeconds(_boxDuration + _interval);
        _dcontroller.BreathEffect.Stop();
    }

    IEnumerator Stun(float duration)
    {
        StunBox stunBox = Managers.Resource.Instantiate("Skill/StunBoxRect").GetComponent<StunBox>();
        stunBox.SetUp(transform, duration);
        stunBox.transform.rotation = _dcontroller.transform.rotation;
        stunBox.transform.position = _dcontroller.transform.position + _pos;
        stunBox.transform.localScale = _scale;

        float timer = 0;
        while (timer <= duration)
        {
            Vector3 moveStep = stunBox.transform.forward * _speed * Time.deltaTime;
            stunBox.transform.position += moveStep;

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        Managers.Resource.Destroy(stunBox.gameObject);
    }

    IEnumerator Breath(float duration)
    {   
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _patternDmg, -1, false, duration);
        hitbox.transform.rotation = _dcontroller.transform.rotation;
        hitbox.transform.position = _dcontroller.transform.position + _pos;
        hitbox.transform.localScale = _scale;

        float timer = 0;
        while (timer <= duration)
        {
            Vector3 moveStep = hitbox.transform.forward * _speed * Time.deltaTime;
            hitbox.transform.position += moveStep;

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        Managers.Resource.Destroy(hitbox.gameObject);
        
    }
}

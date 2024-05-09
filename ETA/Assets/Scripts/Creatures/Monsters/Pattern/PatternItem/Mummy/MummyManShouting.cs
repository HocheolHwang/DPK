using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManShouting : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(6.0f, 6.0f, 1.0f);
    [SerializeField] float _upPos = 1.0f;
    [SerializeField] float _speed = 15.0f;
    [SerializeField] float _intervalTime;       // hitbox가 생성되는 간격

    private MummyManAnimationData _data;
    private Coroutine[] cos;

    public override void Init()
    {
        base.Init();

        _createTime = 0.45f;
        _patternRange = _hitboxRange;
        _patternDmg = 20;

        _data = GetComponent<MummyManAnimationData>();
        _intervalTime = _data.ShoutingAnim.length / 2;

        cos = new Coroutine[2];
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = _controller.transform.position + rootUp;
        float duration = _data.ShoutingAnim.length * 2.0f;

        yield return new WaitForSeconds(_createTime);
        Managers.Sound.Play("Sounds/Monster/Mummy/MummyShouting2_SND", Define.Sound.Effect);

        cos[0] = StartCoroutine(CreateShouting(_attackDamage + _patternDmg, Pos, duration));

        yield return new WaitForSeconds(_intervalTime);
        cos[1] = StartCoroutine(CreateShouting(_attackDamage + _patternDmg, Pos, duration));

        yield return new WaitForSeconds(10.0f);
        //foreach (var c in cos)
        //{
        //    StopCoroutine(c);
        //}
    }

    IEnumerator CreateShouting(int attackDMG, Vector3 Pos, float duration)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Mummy_Shouting, duration, transform);
        ps.transform.position = Pos;
        yield return new WaitForSeconds(0.3f);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, attackDMG, -1, false, duration);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = ps.transform.rotation;
        hitbox.transform.position = Pos;

        float timer = 0;
        // timer가 duration만큼 증가하지 않는 경우가 있다.
        while (timer <= duration - 0.5f)
        {
            Vector3 moveStep = hitbox.transform.forward * _speed * Time.deltaTime;
            hitbox.transform.position += moveStep;
            ps.transform.position += moveStep;

            hitbox.transform.localScale += new Vector3(0.02f, 0.02f, 0);

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
        //Managers.Effect.Stop(ps);
    }
}

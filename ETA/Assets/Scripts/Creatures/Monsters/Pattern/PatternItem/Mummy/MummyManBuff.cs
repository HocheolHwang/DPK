using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MummyManBuff : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _buffRange = new Vector3(2.0f, 2.0f, 2.0f);
    [SerializeField] Vector3 _shieldRange = new Vector3(0.7f, 0.7f, 0.7f);

    private Transform[] _closedMonsterList;
    private float duration;
    private int healAmount;
    private int upAmountATK;
    private int upAmountDEF;
    private int upShield;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternRange = _buffRange;

        // Buff Stat
        duration = 30.0f;
        upAmountATK = 10;
        upAmountDEF = 5;
        healAmount = _controller.Stat.MaxHp / 10;
        upShield = _controller.Stat.MaxHp / 10;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up);

        yield return new WaitForSeconds(_createTime);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Mummy_Buff, duration, null);
        ps.transform.localScale = _patternRange;
        ps.transform.SetParent(_controller.transform);
        ps.transform.position = _controller.transform.position;

        ParticleSystem shieldPS = Managers.Effect.Play(Define.Effect.Mummy_Shield, duration, null);
        shieldPS.transform.localScale = _shieldRange;
        shieldPS.transform.SetParent(_controller.transform);
        shieldPS.transform.position = _controller.transform.position + rootUp;

        _controller.IncreaseDamage(upAmountATK);
        _controller.IncreaseDefense(upAmountDEF);
        _controller.IncreaseHp(healAmount);
        _controller.GetShield(upShield);

        StartCoroutine(DestroyShield(shieldPS, _controller.transform, duration, upShield));
        StartCoroutine(DecreaseStat(duration, _controller.transform));
    }

    // --------------------------- Get the closed Monster using the monsterList ----------------------------------

    IEnumerator DecreaseStat(float afterTime, Transform controller)
    {
        yield return new WaitForSeconds(afterTime);

        if (controller == null) yield break;

        controller.GetComponent<BaseController>().DecreaseDamage(upAmountATK);
        controller.GetComponent<BaseController>().DecreaseDefense(upAmountDEF);
    }

    IEnumerator DestroyShield(ParticleSystem ps, Transform controller, float afterTime, int shield)
    {
        Debug.Log($"Shield : {controller.GetComponent<Stat>().Shield}");

        float timer = 0;
        while (timer <= afterTime)
        {
            if (controller.GetComponent<Stat>().Shield <= 0)
            {
                Managers.Effect.Stop(ps);
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (controller.GetComponent<Stat>().Hp <= 0) yield break;

        controller.GetComponent<BaseController>().RemoveShield(shield);
    }
}

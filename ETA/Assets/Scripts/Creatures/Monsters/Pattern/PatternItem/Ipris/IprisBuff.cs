using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisBuff : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _buffRange = new Vector3(2.0f, 2.0f, 2.0f);
    [SerializeField] Vector3 _shieldRange = new Vector3(0.7f, 0.7f, 0.7f);

    private float duration;
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
        upShield = _controller.Stat.MaxHp / 10;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up);

        yield return new WaitForSeconds(_createTime);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_Buff, duration, null);
        ps.transform.localScale = _patternRange;
        ps.transform.SetParent(_controller.transform);
        ps.transform.position = _controller.transform.position;

        ParticleSystem shieldPS = Managers.Effect.Play(Define.Effect.Mummy_Shield, duration, null);
        shieldPS.transform.localScale = _shieldRange;
        shieldPS.transform.SetParent(_controller.transform);
        shieldPS.transform.position = _controller.transform.position + rootUp;

        _controller.IncreaseDamage(upAmountATK);
        _controller.IncreaseDefense(upAmountDEF);
        _controller.GetShield(upShield);
    }
}

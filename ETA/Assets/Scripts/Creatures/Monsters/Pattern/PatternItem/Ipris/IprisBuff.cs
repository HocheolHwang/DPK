using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisBuff : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _buffRange = new Vector3(2.0f, 2.0f, 2.0f);
    [SerializeField] Vector3 _shieldRange = new Vector3(0.7f, 0.7f, 0.7f);
    [SerializeField] float _upPos = 1.5f;

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
        upAmountATK = 2;
        upAmountDEF = 1;
        upShield = (int)(_controller.Stat.MaxHp * 0.15f);
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);

        yield return new WaitForSeconds(_createTime);

        ParticleSystem ps = Managers.Effect.ContinuePlay(Define.Effect.Ipris_Buff);
        ps.transform.localScale = _patternRange;
        ps.transform.SetParent(_controller.transform);
        ps.transform.position = _controller.transform.position;

        ParticleSystem shieldPS = Managers.Effect.ContinuePlay(Define.Effect.Ipris_Shield);
        shieldPS.transform.localScale = _shieldRange;
        shieldPS.transform.SetParent(_controller.transform);
        shieldPS.transform.position = _controller.transform.position + rootUp;

        _controller.IncreaseDamage(upAmountATK);
        _controller.IncreaseDefense(upAmountDEF);
        _controller.GetShield(upShield);

        StartCoroutine(DestroyShield(shieldPS, _controller.transform));
    }

    IEnumerator DestroyShield(ParticleSystem ps, Transform controller)
    {
        while (true)
        {
            if (controller.GetComponent<Stat>().Shield <= 0)
            {
                Managers.Effect.Stop(ps);
                yield break;
            }
            yield return null;
        }
    }
}

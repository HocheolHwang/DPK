using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// 버프 수치(HP 10% 회복, ATK += 10, DEF += 5, TIME: 30초 )
public class MummyBufferBuff : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 2.0f, 2.0f);

    private Transform[] _closedMonsterList;
    private float duration;
    private int healAmount;
    private int upAmountATK;
    private int upAmountDEF;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternRange = _hitboxRange;

        // Buff Stat
        duration = 30.0f;
        upAmountATK = 10;
        upAmountDEF = 5;
    }

    public override IEnumerator StartPatternCast()
    {
        yield return new WaitForSeconds(_createTime);

        GetClosedMonsters();
        foreach (Transform monster in  _closedMonsterList)
        {
            ParticleSystem ps = Managers.Effect.Play(Define.Effect.Mummy_Buff, duration, null);
            ps.transform.localScale = _patternRange;
            ps.transform.SetParent(monster);
            ps.transform.position = monster.position;

            healAmount = monster.GetComponent<Stat>().MaxHp / 10;

            monster.GetComponent<BaseController>().IncreaseDamage(upAmountATK);
            monster.GetComponent<BaseController>().IncreaseDefense(upAmountDEF);
            monster.GetComponent<BaseController>().IncreaseHp(healAmount);

            StartCoroutine(DecreaseStat(duration, monster));
        }
    }


    // --------------------------- Get the closed Monster using the monsterList ----------------------------------
    private void GetClosedMonsters()    // 일정 거리내에 존재하는 몬스터 가져오기
    {
        Vector3 curPos = _controller.transform.position;
        float detectRange = _controller.Detector.DetectRange;
        Collider[] monsters = Physics.OverlapSphere(curPos, detectRange, LayerMask.GetMask("Monster"));
        if (monsters.Length == 0)
        {
            Debug.Log($"몬스터가 없으면 안 됨, 스스로를 포함해야 하기 때문");
            return;
        }

        _closedMonsterList = new Transform[monsters.Length];

        for (int i = 0; i < monsters.Length; ++i)
        {
            _closedMonsterList[i] = monsters[i].transform;
        }
    }

    IEnumerator DecreaseStat(float afterTime, Transform controller)
    {
        yield return new WaitForSeconds(afterTime);

        if (controller == null) yield break;

        controller.GetComponent<BaseController>().DecreaseDamage(upAmountATK);
        controller.GetComponent<BaseController>().DecreaseDefense(upAmountDEF);
    }
}

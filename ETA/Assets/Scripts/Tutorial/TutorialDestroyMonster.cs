using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDestroyMonster : TutorialBase
{
    [SerializeField]
    private GameObject[] monsters;

    public override void Enter()
    {
        // 파괴해야할 몬스터들을 활성화
        foreach (var monster in monsters)
        {
            monster.SetActive(true);
        }
    }

    public override void Execute(TutorialController controller)
    {
        // 모든 몬스터가 비활성화되었는지 확인
        bool allDestroyed = true;
        foreach (var monster in monsters)
        {
            if (monster != null && monster.activeSelf) // Null 체크 추가
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
    }
}

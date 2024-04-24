using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // UI 버튼에 의해 호출됩니다.
    // 인자로 넘어온 skill 정보에 따라 애니메이션을 플레이하고
    // damage 정보 만큼 피해를 입힙니다.
    public void ActivateSkill(SOSkill skill)
    {
        anim.Play(skill.animationName);
        print(string.Format("적에게 스킬 {0} 로 {1} 의 피해를 주었습니다.", skill.name, skill.damage));

        // 스킬 이펙트 prefab을 로드하고 생성합니다.
        GameObject effectInstance = Instantiate(skill.effectName, transform.position, Quaternion.identity);
        effectInstance.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        effectInstance.transform.localScale *= 1f; // 현재 scale에 상대적인 scale 값으로 설정

        // 일정 시간이 지난 후에 스킬 이펙트를 파괴합니다.
        Destroy(effectInstance, skill.effectDuration);
    }
}

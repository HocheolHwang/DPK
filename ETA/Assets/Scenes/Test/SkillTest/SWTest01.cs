using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWTest01 : ISkill
{
    private Animator _animator;

    public override void InitSetting()
    {
        Debug.Log($"Test SWTest01");
    }

    public override void Using()
    {
        Debug.Log($"Test SWTest01 Using");
        if (_animator != null)
        {
            _animator.CrossFade("Skill01_1", 0.05f);
            Debug.Log($"Test SWTest01 Using");
        }
        else
        {
            Debug.LogError("Animator reference is null!");
        }
    }

    // 애니메이터를 설정하기 위한 메서드 추가
    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }
}

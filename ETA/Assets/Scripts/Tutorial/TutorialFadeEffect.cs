using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFadeEffect : TutorialBase
{
    [SerializeField]
    private FadeEffect  fadeEffect;
    [SerializeField]
    private GameObject  uiElement;

    [SerializeField]
    private bool        isFadeIn = false;
    private bool        isCompleted = false;

    public override void Enter()
    {
        uiElement.SetActive(true); // 페이드 시작 시 UI 활성화
        if ( isFadeIn == true )
        {
            fadeEffect.FadeIn(OnAfterFadeEffect);
        }
        else
        {
            fadeEffect.FadeOut(OnAfterFadeEffect);
        }
    }

    private void OnAfterFadeEffect()
    {
        isCompleted = true;
    }

    public override void Execute(TutorialController controller)
    {
        if ( isCompleted == true )
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit() 
    {
        uiElement.SetActive(false); // 튜토리얼 종료 시 UI 비활성화
    }
}

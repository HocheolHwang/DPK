using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Effect_UI : UI_Popup
{
    private FadeEffect _fadeEffect;
    public override void Init()
    {
        base.Init();
        _fadeEffect = GetComponentInChildren<FadeEffect>();
        _fadeEffect.FadeIn(OnFadeEffect);
    }

    void OnFadeEffect()
    {
        Managers.UI.ClosePopupUI();
    }

    public void FadeIn()
    {
        
    }

    public void FadeOut()
    {
        //_fadeEffect.FadeOut(OnFadeEffect);
    }
}

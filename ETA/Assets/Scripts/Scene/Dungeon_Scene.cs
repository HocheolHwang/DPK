using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon_Scene : BaseScene
{
    private FadeEffect _fadeEffect;
    private void Start()
    {
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/DeepForest");
        Managers.UI.ShowPopupUI<Fade_Effect_UI>("[Common]_Fade_Effect_UI");
        Managers.Resource.Instantiate("MonsterManager").name = "@MonsterManager";
    }
    
    public override void Clear()
    {
        Debug.Log("Dungeon Scene Clear");
    }
}

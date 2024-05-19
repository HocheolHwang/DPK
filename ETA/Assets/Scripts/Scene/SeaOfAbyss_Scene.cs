using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaOfAbyss_Scene : Dungeon_Scene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.StarShardPlain;
        Managers.Sound.Play("BackgroundMusic/SeaOfAbyss", Define.Sound.BGM);
    }
}

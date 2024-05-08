using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;


    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Debug.Log("나는 부모");
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if(obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }

    }
    public abstract void Clear();

}

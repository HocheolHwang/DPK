using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        Managers.Photon.Connect();
    }

    IEnumerator enterPhoton()
    {
        yield return Managers.Photon.IsConnecting;

        Managers.Photon.MakeRoom("ttsets");
        Managers.Photon.CloseRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

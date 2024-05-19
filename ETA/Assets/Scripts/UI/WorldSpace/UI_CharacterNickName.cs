using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CharacterNickName : UI_Base
{
    public string NickName { get; set; }
    TextMeshProUGUI NickNameText;
    enum Texts
    {
        CharacterNickNameText
    }
    public override void Init()
    {
        Debug.Log("123");
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetText((int)Texts.CharacterNickNameText).text = NickName;
    }

    public void SetCharacterName(string name)
    {
        GetText((int)Texts.CharacterNickNameText).text = name;
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
       
        transform.rotation = Camera.main.transform.rotation;
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y) + 0.4f);
    }
}

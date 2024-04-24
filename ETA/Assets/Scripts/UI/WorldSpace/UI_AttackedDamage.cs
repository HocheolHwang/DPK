using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttackedDamage : UI_Base
{

    public int AttackedDamage { get; set; }
    enum Texts
    {
        AttackedDamageText
    }

    Stat _stat;

    public override void Init()
    {
        //Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Get<TextMeshProUGUI>((int)Texts.AttackedDamageText).text = $"{AttackedDamage}";
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        
        Destroy(gameObject, 1.0f);



    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * Time.deltaTime;
        Debug.Log(transform.localScale);
        transform.localScale -= new Vector3(0.00015f, 0.00015f, 0.00015f);
        
    }



}
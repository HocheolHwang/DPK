using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MannequinController : MonoBehaviour
{
    public int index;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {

    }

    public void Init()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void EnterPlayer(string nickName, string classCode)
    {
        Debug.Log("123sadasdasda");
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<TextMeshPro>().text = nickName;
        ClassUpdate(classCode);
    }
    public void SetNickName(string nickName)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<TextMeshPro>().text = nickName;
    }

    public void ClassUpdate(string classCode)
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        if(classCode == "C001")
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(classCode == "C002")
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else if(classCode == "C003")
        {
            Debug.Log("마법사 생성");
            transform.GetChild(3).gameObject.SetActive(true);
        }

    }

}

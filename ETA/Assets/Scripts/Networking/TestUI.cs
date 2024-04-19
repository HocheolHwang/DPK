using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{

    NetworkManager req;

    private void Start()
    {
        req = gameObject.GetComponent<NetworkManager>();
    }
    public void Post()
    {
        ResponseDto test = new ResponseDto();

        test.gold = 124812048;
        test.id = "Tkvl";
        test.nickName = "이싸피";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("POST", "post", testData);
    }

    public void Put()
    {
        ResponseDto test = new ResponseDto();

        test.gold = 124812048;
        test.id = "ssafy";
        test.nickName = "김싸피";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("PUT", "put", testData);
    }

    public void Get()
    {

        req.HTTPCall("GET", "get");
    }

    public void MakeRoom()
    {
        PhotonManager photon = gameObject.GetComponent<PhotonManager>();

        photon.MakeRoom();
    }
}

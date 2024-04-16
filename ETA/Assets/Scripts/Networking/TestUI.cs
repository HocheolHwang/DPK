using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{

    HTTPRequest req;

    private void Start()
    {
        req = gameObject.GetComponent<HTTPRequest>();
    }
    public void Post()
    {
        TESTClass test = new TESTClass();

        test.data = 123;
        test.time = Time.deltaTime;
        test.name = "Á¹·Á";

        string testData = JsonUtility.ToJson(test);

        req.PostCall("post", testData);
    }

    public void Get()
    {

        req.GetCall("get");
    }

    [System.Serializable]
    class TESTClass
    {
        public string name;
        public int data;
        public float time;
    }

    public void MakeRoom()
    {
        PhotonManager photon = gameObject.GetComponent<PhotonManager>();

        photon.MakeRoom();
    }
}



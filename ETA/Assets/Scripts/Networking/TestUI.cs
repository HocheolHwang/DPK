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
        TESTClass test = new TESTClass();

        test.data = 123;
        test.time = Time.deltaTime;
        test.name = "졸려";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("POST", "post", testData);
    }

    public void Put()
    {
        TESTClass test = new TESTClass();

        test.data = 124812048;
        test.time = Time.deltaTime;
        test.name = "이것은 put입니다";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("PUT", "put", testData);
    }

    public void Get()
    {

        req.HTTPCall("GET", "get");
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

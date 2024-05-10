using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 'Player' 태그를 가진 오브젝트가 트리거에 들어왔을 때
        {
            CameraChange(other);
        }
    }

    public void CameraChange(Collider other)
    {
        Camera.main.GetComponent<CameraController>()._player = other.gameObject;
    }
}

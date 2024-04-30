using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float amplitude = 0.5f;  // 움직임의 진폭
    public float frequency = 2.0f;  // 움직임의 빈도

    private CinemachinePOV pov;  // 카메라의 POV 컴포넌트 참조
    private float originalHeight;  // 초기 카메라 높이

    void Start()
    {
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            originalHeight = pov.m_VerticalAxis.Value;
        }
    }

    void Update()
    {
        if (pov != null)
        {
            // Sin 함수를 사용하여 위아래 움직임을 생성
            float newHeight = originalHeight + Mathf.Sin(Time.time * frequency) * amplitude;
            pov.m_VerticalAxis.Value = newHeight;
        }
    }
}

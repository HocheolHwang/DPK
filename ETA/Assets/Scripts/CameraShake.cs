using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeTime = 1.0f;
    public float shakeSpeed = 2.0f;
    public float shakeAmount = 1.0f;

    private Transform cam;
    void Start()
    {
        cam = Camera.main.transform;
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = cam.position;

        float elapsedTime = 0f;
        while (elapsedTime < shakeTime)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);

            cam.localPosition = new Vector3(
                originalPos.x + x,
                originalPos.y + y,
                originalPos.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = originalPos;
    }
}

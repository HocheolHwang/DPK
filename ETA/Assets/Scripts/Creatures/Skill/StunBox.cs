using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// STUN: 3초 고정
public class StunBox : MonoBehaviour
{
    private Transform _attacker;
    private float _duration;

    public void SetUp(Transform attacker, float duration = 0.1f)
    {
        _attacker = attacker;
        _duration = duration;
        StartCoroutine(InActiveColider(_duration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_attacker.gameObject.CompareTag("Monster") && other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"groggy player : {other.gameObject.name}");
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.ChangeState(controller.GROGGY_STATE, true);
        }
    }

    IEnumerator InActiveColider(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    public void SetActiveCollider()
    {
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutHitBox : MonoBehaviour
{
    [Header("Donut 확장에 관련된 변수")]
    [SerializeField] private SphereCollider _outer;
    [SerializeField] private SphereCollider _inner;
    [SerializeField] private float _expandSpeed = 0.5f;

    private void Update()
    {
        _outer.radius += _expandSpeed * Time.deltaTime;
        _inner.radius += _expandSpeed * Time.deltaTime;
    }
}

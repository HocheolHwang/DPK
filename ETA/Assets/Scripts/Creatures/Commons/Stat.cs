using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 몬스터의 공통 스탯
public class Stat : MonoBehaviour
{
    [SerializeField] private int _maxHp;
    [SerializeField] private int _hp;
    [SerializeField] private float _moveSpeed;
    // 방어력?
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackRange;

    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
}

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
    [SerializeField] private int _attackDamage;
    [SerializeField] private int _defense;

    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public int Defense { get => _defense; set => _defense = value; }

}

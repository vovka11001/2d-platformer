using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _health;
    private float _deathCooldown = 1.5f; 

    private Coroutine _DeathCooldownCoroutine;

    public int Damage {get; private set;}
    public bool IsDead { get; private set; }
    
    public event Action Hurt;
    public event Action Death;
    
    private void Start()
    {
        _health = 100;
        Damage = 10;
    }
    
    public void DecreaseHealth(int damage)
    {
        if (IsDead)
            return;
        
        _health -= damage;
        Hurt?.Invoke();
        
        if (_health <= 0)
        {
            DestroyEnemy();
        }
    }
    
    private void DestroyEnemy()
    {
        if (IsDead) 
            return;
        
        Death?.Invoke();
        IsDead = true;
        
        if (_DeathCooldownCoroutine != null)
            StopCoroutine(_DeathCooldownCoroutine);
        
        _DeathCooldownCoroutine = StartCoroutine(DeathCooldown());
    }
    
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(_deathCooldown);
        
        Destroy(gameObject);
    }
}
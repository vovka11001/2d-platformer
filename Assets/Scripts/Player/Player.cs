using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemDetector _itemDetector;
    
    private float _deathCooldown = 1f;
    private List<Coin> _coinsCollected;
    
    private Coroutine _deathCooldownCoroutine;
    
    public bool IsDead { get; private set; }
    public int Damage { get; private set; }    
    public int Health { get; private set; }    
    
    public event Action DecreasedHealth;
    public event Action Death;

    private void OnEnable()
    {
        _itemDetector.TriggerEntered += Collect;
    }

    private void OnDisable()
    {
        _itemDetector.TriggerEntered -= Collect;
    }
    
    private void Start()
    {
        Health = 100;
        Damage = 25;
        transform.position = _spawnPoint.transform.position;
        _coinsCollected = new List<Coin>();
    }
    
    public void DecreaseHealth(int damage)
    {
        if (IsDead) 
            return;

        if (!IsDead)
        {
            Health -= damage;
            DecreasedHealth?.Invoke();
        }

        if (Health <= 0)
        {
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        if (IsDead) 
            return;
        
        Death?.Invoke();
        IsDead = true;
        
        if (_deathCooldownCoroutine != null)
            StopCoroutine(_deathCooldownCoroutine);
        
        _deathCooldownCoroutine = StartCoroutine(DeathCooldown());
    }
    
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(_deathCooldown);
        
        Destroy(gameObject);
    }

    private void Collect(Item item)
    {
        if (item.TryGetComponent(out Coin coin))
        {
            _coinsCollected.Add(coin);
            item.Collect();
        }
        else if (item.TryGetComponent(out MedicineChest medicineChest))
        {
            Health += medicineChest.IncreaseAmount;
            medicineChest.Collect();
        }
    }
}
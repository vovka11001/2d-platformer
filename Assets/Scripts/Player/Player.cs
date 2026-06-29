using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CoinDetector _coinDetector;
    [SerializeField] private MedicineChestDetector _medicineChestDetectorDetector;
    
    private float _deathCooldown = 0.5f;
    private List<Coin> _coinsCollected;
    
    private Coroutine _deathCooldownCoroutine;
    
    public bool IsDead { get; private set; }
    public int Damage { get; private set; }    
    public int Health { get; private set; }    
    
    public event Action DecreasedHealth;
    public event Action Death;

    private void OnEnable()
    {
        _coinDetector.TriggerEntered += CollectCoin;
        _medicineChestDetectorDetector.TriggerEntered += IncreaseHealth;
    }

    private void OnDisable()
    {
        _coinDetector.TriggerEntered -= CollectCoin;
        _medicineChestDetectorDetector.TriggerEntered -= IncreaseHealth;
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

    private void CollectCoin(Coin coin)
    {
        _coinsCollected.Add(coin);
    }

    private void IncreaseHealth(MedicineChest medicineChest)
    {
        Health += medicineChest.IncreaseAmount;
    }
}
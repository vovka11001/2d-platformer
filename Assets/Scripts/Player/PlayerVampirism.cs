using System;
using System.Collections;
using UnityEngine;

public class PlayerVampirism : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _radiusVisual;

    private readonly int _damagePerTick = 5;
    private readonly float _tickInterval = 1f;
    private readonly float _spellDuration = 6f;
    private readonly float _cooldownDuration = 4f;
    private readonly float _detectionRadius = 3f;

    private WaitForSeconds _waitForTick;
    private Coroutine _spellCoroutine;
    private bool _isOnCooldown;

    public event Action<int> DamageDealt;
    public event Action<float, float> DurationChanged;
    public event Action<float, float> CooldownChanged;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        _waitForTick = new WaitForSeconds(_tickInterval);
        _radiusVisual.SetActive(false);
    }

    private void OnDisable()
    {
        if (_spellCoroutine != null)
            StopCoroutine(_spellCoroutine);
    }

    public void TryActivate()
    {
        if (IsActive || _isOnCooldown)
            return;

        _spellCoroutine = StartCoroutine(SpellRoutine());
    }

    private IEnumerator SpellRoutine()
    {
        IsActive = true;
        _radiusVisual.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < _spellDuration)
        {
            Enemy nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                nearestEnemy.TakeDamage(_damagePerTick);
                DamageDealt?.Invoke(_damagePerTick);
            }

            elapsedTime += _tickInterval;
            DurationChanged?.Invoke(_spellDuration - elapsedTime, _spellDuration);

            yield return _waitForTick;
        }

        IsActive = false;
        _radiusVisual.SetActive(false);

        yield return StartCoroutine(CooldownRoutine());

        _spellCoroutine = null;
    }

    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        float elapsedTime = 0f;

        while (elapsedTime < _cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            CooldownChanged?.Invoke(elapsedTime, _cooldownDuration);
            yield return null;
        }

        _isOnCooldown = false;
    }

    private Enemy FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _enemyLayer);

        Enemy nearestEnemy = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy) && enemy.IsDead == false)
            {
                float sqrDistance = ((Vector2)transform.position - (Vector2)enemy.transform.position).sqrMagnitude;

                if (sqrDistance < nearestSqrDistance)
                {
                    nearestSqrDistance = sqrDistance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }
}
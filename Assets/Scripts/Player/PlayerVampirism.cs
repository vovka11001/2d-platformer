using System;
using System.Collections;
using UnityEngine;

public class PlayerVampirism : MonoBehaviour
{
    [SerializeField] private NearestEnemyFinder _enemyFinder;

    private readonly int _damagePerTick = 3;
    private readonly float _tickInterval = 1f;
    private readonly float _spellDuration = 6f;
    private readonly float _cooldownDuration = 4f;

    private WaitForSeconds _waitForTick;
    private Coroutine _spellCoroutine;
    private bool _isOnCooldown;

    public event Action<int> DamageDealt;
    public event Action<float, float> DurationChanged;
    public event Action<float, float> CooldownChanged;
    public event Action Activated;
    public event Action Deactivated;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        _waitForTick = new WaitForSeconds(_tickInterval);
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
        Activated?.Invoke();

        float elapsedTime = 0f;

        while (elapsedTime < _spellDuration)
        {
            Enemy nearestEnemy = _enemyFinder.FindNearest();

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
        Deactivated?.Invoke();

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
}
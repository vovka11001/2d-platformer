using System;

public interface IDamageable 
{
    void TakeDamage(int damage);
    int Health { get; }
    int MaxHealth { get; }
    bool IsDead { get; }
    event Action<int, int> HealthChanged;
}
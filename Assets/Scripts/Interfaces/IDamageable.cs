public interface IDamageable 
{
    void TakeDamage(int damage);
    int Health {  get; }
    bool IsDead { get; }
}
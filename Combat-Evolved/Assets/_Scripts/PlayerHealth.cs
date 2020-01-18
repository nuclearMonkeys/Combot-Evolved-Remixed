using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable currentHP;

    public bool resetHP;
    public FloatReference maxHP;
    public UnityEvent damageEvent;
    public UnityEvent deathEvent;

    void Start() 
    {
        if (resetHP)
            currentHP.SetValue(maxHP);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Bullet"))
            return;
        
        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        if(damage != null) 
        {
            currentHP.ApplyChange(-damage.damageAmount);
            Destroy(other.gameObject);
            damageEvent.Invoke();
        }
        if(currentHP.Value <= 0.0f)
        {
            deathEvent.Invoke();
        }
    }
}

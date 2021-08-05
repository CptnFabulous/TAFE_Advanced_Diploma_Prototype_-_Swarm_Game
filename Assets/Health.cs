using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int current = 5;
    public int max = 5;
    public UnityEvent onDie;
    
    
    public void Damage(int amount)
    {
        Debug.Log("Damaging " + name + " for " + amount + " points");
        current -= amount;
        if (current <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onDie.Invoke();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SpawnCorpse(Corpse c)
    {
        Instantiate(c, transform.position, transform.rotation);
    }
}

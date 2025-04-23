using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    
    private SpriteRenderer spriteRenderer;

    public static event Action OnPlayerDied;
    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Trap trap = collision.GetComponent<Trap>();
        if (trap)
        {
            TakeDamage(trap.damage);
            SoundEffectManager.Play("PlayerHit");
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            OnPlayerDied.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}

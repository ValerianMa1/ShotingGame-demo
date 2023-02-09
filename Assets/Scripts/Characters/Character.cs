using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("----- HEALTH -----")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] StatesBar onHealthBar;
    [SerializeField] bool showOnHealthBar = true;



    [Header("----- DEATH -----")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioData[] deathSFX; 

    protected virtual private void OnEnable() 
    {
        health = maxHealth;//初始状态是满血
        if(showOnHealthBar)ShowOnHealthBar();
        else HideOnHealthBar();
    }

    public void ShowOnHealthBar()
    {
        onHealthBar.gameObject.SetActive(true);
        onHealthBar.Initialize(health,maxHealth);
    }

    public void HideOnHealthBar()
    {
        onHealthBar.gameObject.SetActive(false);
    }


    public virtual void TakeDamage(float damage)
    {
        health -=damage;

        if(showOnHealthBar && gameObject.activeSelf)
        {
            onHealthBar.UpdateStates(health,maxHealth);
        }
        if(health <= 0f)
        {
            Die();
        }
    }


    public virtual void Die()
    {

        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }



    public virtual void RestoreHealth(float value)
    {
        if(health == maxHealth) return ;
        // health +=value;
        // health = Mathf.Clamp(health,0f,maxHealth);
        health = Mathf.Clamp(health += value,0f,maxHealth);//防止生命值超出最大生命值，防止溢出
        if(showOnHealthBar)
        {
            onHealthBar.UpdateStates(health,maxHealth);
        }
    }




    protected IEnumerator HealtgenerateCoroutine(WaitForSeconds cd,float percent)
    {
        while(health <= maxHealth)
        {
            yield return cd;
            RestoreHealth(maxHealth * percent);
        }

    }
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds cd,float percent)
    {
        while(health > 0)
        {
            yield return cd;
            TakeDamage(maxHealth * percent);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;
    [SerializeField] int scorePoint = 100;

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.instance.Obtain(deathEnergyBonus);
        EnemyManager.instance.RemoveFromList(gameObject);
        base.Die();
    }
}

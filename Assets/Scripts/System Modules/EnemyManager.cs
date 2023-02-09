using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null: enemyList[Random.Range(0,enemyList.Count)];
    public int WaveNumber =>waveNumber;
    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject waveUI;
    [SerializeField] float timeBetweenWaves = 2f;
    [SerializeField] float timeBetweenSpawns = 1f;//敌人生成的间隔时间
    int waveNumber = 1;//敌人波数
    int enemyAmount;//每波中生成的敌人数量，随着敌人波数不断增加
    [SerializeField] int minEnemyAmount = 4;

    //需要限定敌人生成数量的最大最小值，用来限定enemyamount的区间
    [SerializeField] int maxEnemyAmount = 10;

    [SerializeField] GameObject[] enemyPrefabs;
    WaitForSeconds waitTimeBetweenSpawns;//协程里面需要waitforsecond类型所以这里定义一个
    WaitForSeconds waitTimeBetweenWaves;
    WaitUntil waitUntillNoEnemy;

    List<GameObject> enemyList;


    protected override void Awake()
    {

        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntillNoEnemy = new WaitUntil(() => enemyList.Count == 0);//lamaba表达式
    }

    IEnumerator Start() //将start改造成协程这样一开始就是执行start协程，先挂起等待直到场景中没有敌人，再挂起执行随机生成敌人协程
    {
        while(spawnEnemy)
        {
            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine)); 
        }
           
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount,minEnemyAmount + waveNumber / 3,maxEnemyAmount);
        //在循环开始前确定具体每波需要生成的敌人数量
        for(int i = 0; i < enemyAmount; i++)
        {
            // var enemy = enemyPrefabs[Random.Range(0,enemyPrefabs.Length)];
            // PoolManager.Release(enemy);
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0,enemyPrefabs.Length)]));//从预制体数组中随机抽取一种敌人来生成,合并成一句代码优化性能！
            yield return waitTimeBetweenSpawns;
        }
        yield return waitUntillNoEnemy;
        waveNumber++;
    }



    public void RemoveFromList(GameObject enemy) =>enemyList.Remove(enemy);
}

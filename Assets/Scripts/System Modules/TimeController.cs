using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0,3)] float bulletTimeScale = 0.1f;
    float t;

    float defaultFixDeltaTime;

    protected override void Awake()
    {
        base.Awake();
        defaultFixDeltaTime = Time.deltaTime;
    }


    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }




    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;//只这样写会变得一卡一卡的，因为之前移动携程的固定帧时间是固定的所以，到一定时间才会运行一次，看上去就卡卡的
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration,float outDuration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration,float keepingDuration ,float outDuration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowInAndOutCoroutine(inDuration, keepingDuration ,outDuration));
    }



    IEnumerator SlowInAndOutCoroutine(float inDuration,float keepingDuration ,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        while(t < 1f)
        {
            Time.fixedDeltaTime = defaultFixDeltaTime * Time.timeScale;
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale,1,t);
            yield return null;
        }
    }
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;
        while(t < 1f)
        {
            Time.fixedDeltaTime = defaultFixDeltaTime * Time.timeScale;
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(1,bulletTimeScale,t);
            yield return null;
        }
    }
}

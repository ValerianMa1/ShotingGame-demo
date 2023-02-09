
using System.ComponentModel;
using System.Transactions;
using System.Net.Http.Headers;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//可以直接在player栏里添加，也可以代码敲
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StateBar_HUD stateBar_HUD;
    [SerializeField] float healthRegenerateTime;
    [SerializeField,Range(0f,1f)] float healthRegeneratePercent;
    [SerializeField] bool regenerateHealth = true;

    [Header("----- INPUT -----")]
    [SerializeField] PlayerInput input;

    [Header("----- MOVE -----")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 0.35f;
    [SerializeField] float decelerationTime = 0.75f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] public Rigidbody2D rb;



    bool isOverdrive = false;
    bool isDodging = false;
    float paddingY;
    float paddingX;
    float t;
    float dodgeDuration;
    float currentRoll;
    readonly float slowMotionDuration = 0.5f;

    Vector2 previousVelocity;
    Quaternion previousRotation;
    new Collider2D collider;

    WaitForSeconds waitDecelerationTime;
    WaitForSeconds waitforOverdriveFireInterval;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitHealthRegenerateime;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();


    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;



    [Header("----- Dodge -----")]
    [SerializeField] AudioData dodgeSFX;
    [SerializeField,Range(0f,100f)] int dodgeEnergyCost = 20;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f,0.5f,0.5f);


    [Header("----- FIRE -----")]


    [SerializeField] GameObject projectileOverdrive;
    [SerializeField] GameObject projectile_Middle;
    [SerializeField] GameObject projectile_Top;
    [SerializeField] GameObject projectile_Bottom;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] AudioData projectileSFX;
    [SerializeField] float fireInterval = 0.2f;
    [SerializeField, Range(0, 2)] int weaponPower = 0;



    [Header("----- OVERDRIVE -----")]
    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;
    [SerializeField] int overdriveDodgeFactor = 2;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;


        dodgeDuration = maxRoll / rollSpeed;
        rb.gravityScale = 0f;//重力设为0

        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        waitforOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateime = new WaitForSeconds(healthRegenerateTime);
        waitForFireInterval = new WaitForSeconds(fireInterval);
        
    }


    private protected override void OnEnable()
    {
        //订阅事件
        base.OnEnable();
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }
    private void OnDisable()
    {
        //订阅事件
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    void Start()
    {

        
        stateBar_HUD.Initialize(health,maxHealth);
        input.EnableGameplayInput();//启用动作表
        // TakeDamage(100f);
    }

    void Update()
    {

    }

    #region HEALTH
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        stateBar_HUD.UpdateStates(health,maxHealth);
        TimeController.instance.BulletTime(slowMotionDuration);
        if(gameObject.activeSelf)
        {
            if(regenerateHealth)
            {
                if(healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine =  StartCoroutine(HealtgenerateCoroutine(waitHealthRegenerateime,healthRegeneratePercent));
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        stateBar_HUD.UpdateStates(health,maxHealth);
    }

    public override void Die()
    {
        stateBar_HUD.UpdateStates(0f,maxHealth);
        base.Die();
    }




    #endregion

    #region MOVE
    void Move(Vector2 moveinput)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveinput.y, Vector3.right);//*moveinput.y会产生不同方向的选装，Vector3.right就是x轴
        moveCoroutine = StartCoroutine(StartMoveCoroutine(accelerationTime, moveinput.normalized * moveSpeed, moveRotation));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

    void StopMove()
    {
        //这里是stopcoroutine的第三个重载，传入的参数类型是一个协程，因为加速协程中有本身自己的传入参数，所以协程名和协程函数都不能作为stopcoroutine的参数。
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(StartMoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }


    IEnumerator MoveRangeLimatationCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    IEnumerator StartMoveCoroutine(float time, Vector2 movevelocity, Quaternion moveRotation)//协程控制玩家加速和减速的时间
    {
        t = 0f;
        previousVelocity = rb.velocity;
        previousRotation = transform.rotation;
        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rb.velocity = Vector2.Lerp(previousVelocity, movevelocity, t / time);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;
        StopCoroutine(nameof(MoveRangeLimatationCoroutine));
    }


    #endregion

    #region FIRE


    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));//字符串不会自动补齐，这里nameof更方便呢
    }

    IEnumerator FireCoroutine()
    {
        while (true)//这样可以一直射，但是得想清楚怎么停止死循环，不然游戏会被卡死
        {
            // switch (weaponPower)
            // {
            //     case 0:
            //         Instantiate(projectile_Middle, muzzleMiddle.position, Quaternion.identity);
            //         break;
            //     case 1:
            //         Instantiate(projectile_Middle, muzzleTop.position, Quaternion.identity);
            //         Instantiate(projectile_Middle, muzzleBottom.position, Quaternion.identity);
            //         break;
            //     case 2:
            //         Instantiate(projectile_Middle, muzzleMiddle.position, Quaternion.identity);
            //         Instantiate(projectile_Top, muzzleTop.position, Quaternion.identity);
            //         Instantiate(projectile_Bottom, muzzleBottom.position, Quaternion.identity);
            //         break;
            //     default:
            //         break;

            // }
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Middle,muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Middle,muzzleBottom.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Middle,muzzleTop.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Bottom,muzzleBottom.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Middle,muzzleMiddle.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile_Top,muzzleTop.position);
                    break;
                default:
                    break;

            }
            AudioManager.Instance.PlaySFX(projectileSFX);
            // yield return waitForFireInterval;
            //不应该在循环里进行new操作的，这样每循环一次都会生成一个新变量，卡死！这里再awake函数里就声明了
            yield return isOverdrive ? waitforOverdriveFireInterval : waitForFireInterval;
        }
    }

    #endregion

    #region DODGE

void Dodge()
{
    if(isDodging || PlayerEnergy.instance.IsEnough(dodgeEnergyCost)) return;
    StartCoroutine(nameof(DodgeCoroutine));
}


IEnumerator DodgeCoroutine()
{
    isDodging = true;
    AudioManager.Instance.PlayRandomSFX(dodgeSFX);
    //消耗玩家能量值
    PlayerEnergy.instance.Use(dodgeEnergyCost);
    //Make player invinciable 让玩家无敌
    collider.isTrigger = true;
    //Make player roatate along x axis 让玩家沿着y轴旋转

    currentRoll = 0;//先置零否者出现只能闪避一次的情况
    //TimeController.instance.BulletTime(slowMotionDuration,slowMotionDuration);
    
    

    var scale = transform.localScale;//旋转和缩放是同时发生的，可以直接放进旋转的while循环中


    while(currentRoll < maxRoll)
    {
        currentRoll += rollSpeed * Time.deltaTime;
        transform.rotation =  Quaternion.AngleAxis(currentRoll,Vector3.right);

        if(currentRoll < maxRoll / 2)
        {
            scale -= (Time.deltaTime * 0.8f / dodgeDuration) * Vector3.one;
        }
        else
        {
            scale += (Time.deltaTime * 0.8f/ dodgeDuration) * Vector3.one; 
        }

        transform.localScale = scale;
        //Change player's scale 改变玩家的缩放值
        // var t1 = 0f;
        // var t2 = 0f;
        // if(currentRoll < maxRoll / 2)
        // {
        //     t1 += Time.deltaTime / dodgeDuration;
        //     transform.localScale = Vector3.Lerp(transform.localScale,dodgeScale,t1);
        // }
        // else
        // {
        //     t2 += Time.deltaTime;
        //     transform.localScale = Vector3.Lerp(transform.localScale,Vector3.one,t2);
        // }
        yield return null;
    }   
    collider.isTrigger = false;
    isDodging = false;
}

#endregion

    #region OEVRDRIVE
    

    void Overdrive()
    {
        if(PlayerEnergy.instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }


    //需要实现能量爆发时，玩家移动速度增加，开火间隔减小，但是闪避消耗增加
    void OverdriveOn()
    {
        isOverdrive = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.instance.BulletTime(slowMotionDuration,slowMotionDuration);
    }


    void OverdriveOff()
    {   
        isOverdrive = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }


    #endregion 
}

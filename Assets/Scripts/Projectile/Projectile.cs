using System.Dynamic;
using System.Collections;
using UnityEngine;



public class Projectile : MonoBehaviour
{   
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX;
    [SerializeField] float damage;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;
    protected GameObject target;

    protected virtual void OnEnable() 
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        
        while(gameObject.activeSelf)//条件是物体条件在启用的情况下
        {
            Move();
            yield return null;
        }
    }



    public void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);//不依赖刚体组件，降低游戏性能消耗
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))//这里可以抓取与之产生碰撞的指定组件--character，返回值是boolean
        {
            character.TakeDamage(damage);


            // var contactPoint = collision.GetContact(0);
            // PoolManager.Release(hitVFX,contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            //参数0的返回值是第一次碰撞发生的碰撞点contact2d类型
            //.point就是这个接触点的position,.normal是二位向量值，是碰撞点的法线方向，特效的朝向
            PoolManager.Release(hitVFX,collision.GetContact(0).point,Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }


    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }


}

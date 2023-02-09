using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    [Header("limited")]
    float minX;
    float maxX;
    float minY;
    float maxY;
    float MiddleX;


    void Start()
    {
        Camera mainCamera = Camera.main;
        //玩家位置是个世界位置，必须用过camera类的ViewportToWorldPoint，将视口坐标转换为世界坐标才能使用
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f,0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f,1f));
        MiddleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f,0f,0f)).x;
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        
    }


    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        //用mathf.Clamp可以将浮点数限定在一个限定区域内
        position.x = Mathf.Clamp(playerPosition.x,minX + paddingX,maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y,minY + paddingY,maxY - paddingY);


        return position;
    }


    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY,maxY - paddingY);

        return position;
    }

    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(MiddleX,maxX - paddingX);
        position.y = Random.Range(minY + paddingY,maxY - paddingY);
        return position;
    }


}

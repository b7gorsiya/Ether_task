using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

    // used in case wanted to allow touch or not
    [SerializeField]
    bool allowTouch = true;

    [Header("Boomerang Configuration")]
    [Tooltip("Indicates the max circular motion of boomerang")]
    [SerializeField]
    float maxWidth=6;
    [Tooltip("Indicates the max throw distance of boomerang ")]
    [SerializeField]
    float maxFarValue=10;
    [Tooltip("Indicates the  min circular motion of boomerang")]
    [SerializeField]
    float minWidth = 3;
    [Tooltip("Indicates the min throw distance of boomerang ")]
    [SerializeField]
    float minFarValue = 5;

    [SerializeField]
    float defaultWidth;
    [SerializeField]
    float defaultDistance;

    //time takes to come back to player
    [SerializeField]
    float timeTocomeBack;

    Touch _touch;

    //Store Start x Pos
    float startX;
    //Store Start Y Pos
    float startY;

    // Store diffrences
    float x_diff;
    float y_diff;

    public delegate void ProjectileData(float width,float fardistance,float time);
    public static event ProjectileData DrawProjectile;
    
    public delegate void ThrowBoomerang(float width, float fardistance, float time);
    public static event ThrowBoomerang Throw_Boomer;

    void Update()
    {
        if (allowTouch && Input.touchCount > 0)
        {
            HandleTouch();
        }
    }

    void HandleTouch()
    {
        //store only single touch
        _touch = Input.touches[0];
        if(_touch.phase==TouchPhase.Began)
        {
            startY = _touch.position.y;
            startX = _touch.position.x;
        }else if(_touch.phase==TouchPhase.Moved)
        {
            DrawProjectile(CalculateWidth(_touch.position), CalculateDistance(_touch.position), timeTocomeBack);
          
        }else if(_touch.phase==TouchPhase.Ended)
        {
            Throw_Boomer(CalculateWidth(_touch.position), CalculateDistance(_touch.position), timeTocomeBack);
        }
    }

   
    float CalculateWidth(Vector2 currentPos)
    {
        // narrow down the circular motion when moving Left
        if (currentPos.x<startX)
        {
            x_diff = (startX - currentPos.x)/100;
            x_diff = ((defaultWidth- x_diff) < minWidth) ? minWidth : defaultWidth - x_diff;
        }
        // wider circular motion when moving Left
        else if (currentPos.x > startX)
        {
            x_diff = (currentPos.x - startX) / 100;
            x_diff = ((defaultWidth + x_diff) > maxWidth) ? maxWidth : defaultWidth + x_diff;
        }
        return x_diff;
    }


    float CalculateDistance(Vector2 currentPos)
    {
        // decress distance
        if (currentPos.y < startY)
        {
            y_diff = (startY - currentPos.y)/10;
            y_diff = ((defaultDistance + y_diff) > maxFarValue) ? maxFarValue : defaultDistance + y_diff;
        }
        //incress distance
        else if (currentPos.y > startY)
        {
            y_diff = (currentPos.y- startY)/10;
            y_diff = ((defaultDistance - y_diff) < minFarValue) ? minFarValue : defaultDistance - y_diff;
        }
        return y_diff;
    }
}

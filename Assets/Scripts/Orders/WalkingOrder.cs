using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingOrder : MonoBehaviour
{

    private Order holdingOrder;
    private OrderPost targetPost;
    private Vector2 targetPos;
    private float speed;
    private bool shouldMove;

    private bool isOver = false;


    private void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) < 0.5f)
            {
                shouldMove = false;
                HitTargetPosition();
            }
        }
        else if (isOver)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) < 0.5f)
            {
                shouldMove = false;
                Destroy(gameObject);
            }
        }
    }

    public void Init(Order order,OrderPost post, float speed,Vector2 startPos)
    {
        transform.position = startPos;
        holdingOrder = order;
        targetPost = post;
        this.speed = speed;
        targetPost.AddWalkingOrder(this);
        shouldMove = true;
    }

    public void SetNextPos(Vector2 pos)
    {
        targetPos = pos;
        shouldMove = true;
    }

    public Order GetHoldingOrder()
    {
        return holdingOrder;
    }

    public void WalkToDeath(Vector2 pos)
    {
        isOver = true;
        shouldMove = false;
        targetPos = pos;
    }

    private void HitTargetPosition()
    {
        targetPost.WalkingOrderReachedPoint(this);
    }

}
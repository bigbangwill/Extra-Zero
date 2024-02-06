using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : SingletonComponent<PlayerMovement>
{

    #region Sinleton
    public static PlayerMovement Instance
    {
        get { return ((PlayerMovement) _Instance); }
        set { _Instance = value; }
    }
    #endregion


    [SerializeField] private float movementSpeed;

    private Rigidbody2D rb;

    private NavMeshAgent agent;
    private Coroutine currentPendingCoroutine;
    private NavMeshPath path;

    private NavmeshReachableInformation currentNavInfo;

    private void Start()
    {
        path = new NavMeshPath();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    private void FixedUpdate()
    {
        Vector2 movementVector = MovementManager.Instance.MovementInput() * movementSpeed;
        rb.velocity = movementVector * Time.fixedDeltaTime;
    }


    public void MovetoTarget(NavmeshReachableInformation info)
    {
        Vector2 dis = info.GetDistination();
        bool isReachable = agent.CalculatePath(dis, path);
        if (isReachable && path.status == NavMeshPathStatus.PathComplete)
        {
            if (currentPendingCoroutine != null)
            {
                StopCoroutine(currentPendingCoroutine);
            }
            agent.SetDestination(dis);
            currentPendingCoroutine = StartCoroutine(Moveto(dis));
            currentNavInfo = info;
        }
        else
        {
            Debug.Log("Cant reach the target");
            return;
        }
    }
    public void MovetoTarget(Vector2 target)
    {
        currentNavInfo = null;
        bool isReachable = agent.CalculatePath(target, path);
        if (isReachable && path.status == NavMeshPathStatus.PathComplete)
        {
            if (currentPendingCoroutine != null)
            {
                StopCoroutine(currentPendingCoroutine);
            }
            agent.SetDestination(target);
            currentPendingCoroutine = StartCoroutine(Moveto(target));
        }
        else
        {
            Debug.Log("Cant reach the target");
            return;
        }
    }


    private IEnumerator Moveto(Vector2 pos)
    {
        while (true)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Reached();
                yield return null;
            }
            agent.SetDestination(pos);
            yield return null;
        }
    }

    private void Reached()
    {
        Debug.Log("reached");
        if (currentPendingCoroutine != null)
        {
            StopCoroutine(currentPendingCoroutine);

            if (currentNavInfo != null)
            {
                currentNavInfo.GetCallingMethod().Invoke();
            }
        }
    }




}
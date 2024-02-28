using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Rigidbody2D rb;

    private NavMeshAgent agent;
    private Coroutine currentPendingCoroutine;
    private NavMeshPath path;

    private NavmeshReachableInformation currentNavInfo;

    private PlayerMovementRefrence refrence;

    private void SetRefrence()
    {
        refrence = (PlayerMovementRefrence)FindSORefrence<PlayerMovement>.FindScriptableObject("Player Movement Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        path = new NavMeshPath();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
            Debug.Log(transform.position + " Transfoom pos");
            Debug.Log(target + " Target");
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
            agent.SetDestination(pos);
            yield return new WaitUntil(() => Vector2.Distance(transform.position, pos) <= agent.stoppingDistance);
            Reached();
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
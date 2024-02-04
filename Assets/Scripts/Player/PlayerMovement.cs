using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    private Rigidbody2D rb;

    private NavMeshAgent agent;
    private Coroutine currentPendingCoroutine;
    private NavMeshPath path;


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


    public void MovetoTarget(Vector2 pos)
    {
        bool isReachable = agent.CalculatePath(pos, path);
        if (isReachable && path.status == NavMeshPathStatus.PathComplete)
        {
            if (currentPendingCoroutine != null)
            {
                StopCoroutine(currentPendingCoroutine);
            }
            agent.SetDestination(pos);
            currentPendingCoroutine = StartCoroutine(Moveto(pos));
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
        }
    }




}
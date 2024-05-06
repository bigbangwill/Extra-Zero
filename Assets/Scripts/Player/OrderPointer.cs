using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OrderPointer : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Transform post;

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, 0);
        transform.up = post.position - transform.position;
        //transform.LookAt(post);
        //transform.eulerAngles = new Vector3(0,transform.rotation.eulerAngles.y,0);
    }


}

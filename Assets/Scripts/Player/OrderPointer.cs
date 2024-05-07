using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrderPointer : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Transform post;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer flashSprite;

    [SerializeField] private float fadeSpeed;

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, 0);
        transform.up = post.position - transform.position;
    }


    public void Fade()
    {
        StartCoroutine(StartFade());
    }


    private IEnumerator StartFade()
    {
        while (true)
        {
            Color start = spriteRenderer.color;
            start.a -= fadeSpeed * Time.deltaTime;
            if (start.a <= 0.1f)
            {
                start.a = 0.1f;
                StartCoroutine(StartLight());
                spriteRenderer.color = start;
                flashSprite.color = start;
                yield break;
            }
            spriteRenderer.color = start;
            flashSprite.color = start;
            yield return null;
        }
    }

    private IEnumerator StartLight()
    {
        while (true)
        {
            Color start = spriteRenderer.color;
            start.a += fadeSpeed * Time.deltaTime;
            if (start.a >= 1f)
            {
                start.a = 1f;
                spriteRenderer.color = start;
                flashSprite.color = start;
                yield break;
            }
            spriteRenderer.color = start;
            flashSprite.color = start;
            yield return null;
        }
    }
}
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


    private bool isFadingStart = false;
    private bool isFadingEnding = false;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, 0);
        transform.up = post.position - transform.position;
    }


    public void Fade()
    {
        StartCoroutine(StartFade());
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
        flashSprite.enabled = true;
        Color sprite = spriteRenderer.color;
        sprite.a = 1f;
        Color flash = flashSprite.color;
        flash.a = 1f;
        spriteRenderer.color = sprite;
        flashSprite.color = flash;
        
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
        flashSprite.enabled = false;
        if (isFadingStart)
        {
            StopCoroutine(StartFade());
            isFadingStart = false;
        }
        if (isFadingEnding)
        {
            StopCoroutine(StartLight());
            isFadingEnding = false;
        }
    }



    private IEnumerator StartFade()
    {
        while (true)
        {
            isFadingStart = true;
            Color start = spriteRenderer.color;
            start.a -= fadeSpeed * Time.deltaTime;
            if (start.a <= 0.1f)
            {
                isFadingStart = false;
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
            isFadingEnding = true;
            Color start = spriteRenderer.color;
            start.a += fadeSpeed * Time.deltaTime;
            if (start.a >= 1f)
            {
                isFadingEnding = false;
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
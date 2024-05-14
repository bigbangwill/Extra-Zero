using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrderSlotShow : MonoBehaviour
{
    private SpriteRenderer image;
    [SerializeField] private TextMeshProUGUI stack;
    [SerializeField] private TextMeshProUGUI itemName;

    public enum OrderState { notFilled,filled,failed}


    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
        Reset();
    }

    public void SetOrder(ItemBehaviour target,OrderState state)
    {
        image.sprite = target.IconRefrence();
        itemName.text = target.GetName();
        if (target.IsStackable())
        {
            stack.text = target.CurrentStack().ToString();
        }
        else
        {
            stack.text = string.Empty;
        }
        switch (state) 
        {
            case OrderState.filled: image.color = Color.green; break;
            case OrderState.failed: image.color = Color.red; break;
            case OrderState.notFilled: image.color = Color.white; break;
            default: break;            
        }
    }

    public void Reset()
    {
        image.sprite = null;
        stack.text = string.Empty;
        image.color = Color.white;
        itemName.text= string.Empty;
    }
}
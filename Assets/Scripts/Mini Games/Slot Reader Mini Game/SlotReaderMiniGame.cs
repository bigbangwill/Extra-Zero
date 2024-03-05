using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotReaderMiniGame : MonoBehaviour
{
    [SerializeField] private Transform mouse;

    private float dif = 0;

    private BasementManagerRefrence basementManagerRefrence;
    private SlotReaderManagerRefrence slotReaderManagerRefrence;

    private void LoadSORefrence()
    {
        basementManagerRefrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
        slotReaderManagerRefrence = (SlotReaderManagerRefrence)FindSORefrence<SlotReaderManager>.FindScriptableObject("SlotReader Manager Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
    }

    // Update is called once per frame
    public void MouseIsPressed(PointerEventData eventData)
    {
        mouse.position = eventData.position;
        Vector3 upAxis = new(0, 0, -1);

        float oldRotation = transform.eulerAngles.x;
        transform.LookAt(mouse, upAxis);

        float currentZRotation = transform.eulerAngles.x;
        float rotationDifferenceZ = currentZRotation - oldRotation;
        rotationDifferenceZ = Mathf.Abs(rotationDifferenceZ);
        if (rotationDifferenceZ > 180 || rotationDifferenceZ < -180)
            return;
        dif += rotationDifferenceZ;
        if (dif >= 360)
        {
            slotReaderManagerRefrence.val.SecondElapsed();
            dif = 0;
        }

    }


}
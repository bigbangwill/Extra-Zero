using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotReaderMiniGame : MonoBehaviour
{
    [SerializeField] private Transform mouse;

    private float maxDif = 1440;
    private float dif = 0;

    private BasementManagerRefrence basementManagerRefrence;
    private SlotReaderManagerRefrence slotReaderManagerRefrence;

    private SlotReaderMiniGameRefrence refrence;

    private void SetSORefrence()
    {
        refrence = (SlotReaderMiniGameRefrence)FindSORefrence<SlotReaderMiniGame>.FindScriptableObject("Slot Reader Mini Game Refrence");
        if(refrence == null )
        {
            Debug.LogWarning("Couldnt Find it");
        }
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        basementManagerRefrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
        slotReaderManagerRefrence = (SlotReaderManagerRefrence)FindSORefrence<SlotReaderManager>.FindScriptableObject("SlotReader Manager Refrence");
    }

    private void Awake()
    {
        SetSORefrence();
    }

    private void Start()
    {
        LoadSORefrence();
    }

    public void UpgradeOrbit(bool isQubit)
    {
        if (!isQubit)
            maxDif = 720;
        else
            maxDif = 360;
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
        if (dif >= maxDif)
        {
            slotReaderManagerRefrence.val.SecondElapsed();
            dif = 0;
        }

    }


}
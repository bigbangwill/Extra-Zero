using UnityEngine;

public class RotationTest : MonoBehaviour
{
    [SerializeField] private Transform mouse;

    private float dif = 0;

    // Update is called once per frame
    public void MouseIsPressed()
    {
        mouse.position = BasementManager.Instance.MousePos();
        Vector3 upAxis = new Vector3(0, 0, -1);

        float oldRotation  = transform.eulerAngles.x;
        transform.LookAt(mouse, upAxis);

        float currentZRotation = transform.eulerAngles.x;
        float rotationDifferenceZ = currentZRotation - oldRotation;
        rotationDifferenceZ = Mathf.Abs(rotationDifferenceZ);
        if (rotationDifferenceZ > 180 || rotationDifferenceZ < -180)
            return;
        dif += rotationDifferenceZ;
        if (dif >= 360)
        {
            SlotReaderManager.Instance.SecondElapsed();
            dif = 0;
        }

    }

}
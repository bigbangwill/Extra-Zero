using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerReference playerReference = null;

    void Awake()
    {
        if (playerReference == null)
        {
            Debug.Log("Didnt find it");
            return;
        }
        Debug.Log("We did find it");
        playerReference.val = this;
    }


    public void FUCKYOU()
    {
        Debug.Log("FUCK YOU?????");
    }
}
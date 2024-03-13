using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlchemyRewardPanel : MonoBehaviour
{
    [SerializeField] private Image icon1;
    [SerializeField] private TextMeshProUGUI potionEffectText1;

    [SerializeField] private Image icon2;
    [SerializeField] private TextMeshProUGUI potionEffectText2;

    [SerializeField] private Image icon3;
    [SerializeField] private TextMeshProUGUI potionEffectText3;

    [SerializeField] private GameObject critStatus;
    [SerializeField] private AlchemyMiniGame alchemyMiniGameScript;

    private PotionEffect potionEffect1;
    private PotionEffect potionEffect2;
    private PotionEffect potionEffect3;

    private PotionItem targetPotionItem;

    private PlayerInventoryRefrence inventoryRefrence;
    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
    }

    public void EnableReward(List<PotionEffect> rewardEffects,bool isCrit, PotionItem prePotion)
    {
        if (rewardEffects == null)
        {
            potionEffectText1.text = "No Effect Click To move on!";
            potionEffectText2.text = "No Effect Click To move on!";
            potionEffectText3.text = "No Effect Click To move on!";

            Debug.Log("List is null");
            return;
        }
        critStatus.SetActive(isCrit);


        targetPotionItem = prePotion;
        SetPotionEffects(rewardEffects);
        icon1.sprite = potionEffect1.sprite;
        icon2.sprite = potionEffect2.sprite;
        icon3.sprite = potionEffect3.sprite;
        potionEffectText1.text = potionEffect1.GetEffectName();
        potionEffectText2.text = potionEffect2.GetEffectName();
        potionEffectText3.text = potionEffect3.GetEffectName();

    }

    // This method is for finding random effects to put in the list as the last potion effect the player can find.
    private void SetPotionEffects(List<PotionEffect> potionEffects)
    {
        if (potionEffects == null)
        {
            Debug.Log("Empty Effects");
            return;
        }
        Debug.Log(potionEffects.Count);
        PotionEffect[] finalArray = new PotionEffect[3];
        for(int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, potionEffects.Count - 1);
            finalArray[i] = (potionEffects[random]);
            potionEffects.Remove(potionEffects[random]);
        }
        potionEffect1 = finalArray[0];
        potionEffect2 = finalArray[1];
        potionEffect3 = finalArray[2];
    }

    /// <summary>
    /// This method gets called from the reward panel to set the chosen reward.
    /// </summary>
    /// <param name="rewardNumber"></param>
    public void RewardClicked(int rewardNumber)
    {
        if (potionEffect1 == null && potionEffect2 == null && potionEffect3 == null)
        {
            Debug.Log("Nothing is there to choose");
            //ResetBackToStart();
        }


        PotionEffect targetedEffect;
        switch (rewardNumber) 
        {
            case 1:  targetedEffect = potionEffect1; break;
            case 2:  targetedEffect = potionEffect2; break;
            case 3:  targetedEffect = potionEffect3; break;
            default: targetedEffect = potionEffect1;Debug.LogWarning("Check here asap"); break;
        }

        if (targetedEffect != null)
        {
            Debug.LogWarning("HEREEE");
            targetPotionItem.SetNextEffect(targetedEffect);
        }

        if (inventoryRefrence.val.HaveEmptySlot(targetPotionItem, false))
        {
            inventoryRefrence.val.HaveEmptySlot(targetPotionItem, true);
            ResetBackToStart();
            Debug.Log("Sent");
        }
        else
        {
            Debug.Log("No empty slot");
        }

        if (critStatus.activeSelf)
        {
            if (inventoryRefrence.val.HaveEmptySlot(targetPotionItem, false))
            {
                inventoryRefrence.val.HaveEmptySlot(targetPotionItem, true);
                ResetBackToStart();
                Debug.Log("Sent");
            }
            else
            {
                Debug.Log("No empty slot");
            }
        }
    }

    private void ResetBackToStart()
    {
        icon1.sprite = null;
        icon2.sprite = null;
        icon3.sprite = null;
        potionEffect1 = null;
        potionEffect2 = null;
        potionEffect3 = null;
        potionEffectText1.text = string.Empty;
        potionEffectText2.text = string.Empty;
        potionEffectText3.text = string.Empty;
        critStatus.SetActive(false);
        gameObject.SetActive(false);
        alchemyMiniGameScript.ResetBackToStart();
    }
}
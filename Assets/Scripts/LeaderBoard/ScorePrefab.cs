using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePrefab : MonoBehaviour
{
    [SerializeField] private Image medalIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetScore(Sprite medal, string name, string score)
    {
        medalIcon.sprite = medal;
        nameText.text = name;
        scoreText.text = score;
    }
}

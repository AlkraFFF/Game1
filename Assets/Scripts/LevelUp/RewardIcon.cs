using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class RewardIcon : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text lvlText;

    private RewardSO curReward;

    public void Setup(RewardSO reward, int level)
    {
        curReward = reward;
        iconImg.sprite = reward.Icon;

        if (level + 1 == curReward.MaxLevel)
        {
            lvlText.text = $"Lv Max";
        }
        else
        {
            lvlText.text = $"Lv {level + 1}";
        }
        float curModifier = level == 0 ? reward.StartModifier : reward.LevelModifier;
        descriptionText.text = $"{reward.Description} {reward.StartModifier + reward.LevelModifier * level}" +
            $"(<color=blue>+{curModifier}</color>)";
    }
}  

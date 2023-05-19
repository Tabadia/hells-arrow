using UnityEngine;
using UnityEngine.UI;

public class SpriteToggle : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;

    [SerializeField] private Image targetButton;

    public void ChangeSprite()
    {
        if (targetButton.sprite == buttonSprites[0])
        {
            targetButton.sprite = buttonSprites[1];
            return;
        }

        targetButton.sprite = buttonSprites[0];
    }
}
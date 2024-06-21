using UnityEngine;

public class ChangeSexPlayer : MonoBehaviour
{

    public Sprite WomanSprite;
    public Sprite WomanSpriteHip;

    public Sprite ManSprite;
    public Sprite ManSpriteHip;


    public SpriteRenderer PlayerBody;
    public SpriteRenderer PlayerHip;

    private string _characterGender = "";
    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.GetString("Gender", _characterGender) == "Man")
        {
            PlayerBody.sprite = ManSprite;
            PlayerHip.sprite = ManSpriteHip;
        }
        else
        {
            PlayerBody.sprite = WomanSprite;
            PlayerHip.sprite = WomanSpriteHip;
        }
        
    }

}

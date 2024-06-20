using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    public bool destroy;
    public Image imagePopUp;
    public TextMeshProUGUI namePopUp;

    private void Start()
    {
        destroy = false;
    }

    private void Update()
    {
        if (destroy)
        {
            PopUpManager.Instance.indexItemPlacementAnim -= 1;
            PopUpManager.Instance.NextPopUp();
            Destroy(gameObject);
        }
    }

    public void SetPosition(int index)
    {
        gameObject.transform.localPosition = new Vector3(0, (-112.5f * index) - 37.5f, 0);
    }

    public void SetImageNamePopUp(Sprite image, int quantity)
    {
        imagePopUp.sprite = image;
        string sign = "+";
        if (quantity < 0)
        {
            sign = "";
        }
        namePopUp.text = sign + quantity.ToString();
    }
}
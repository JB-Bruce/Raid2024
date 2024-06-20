using UnityEngine;

public class RansomPaper : MonoBehaviour
{
    [SerializeField]
    private GameObject _ransomPaper;

    public static RansomPaper instance;

    //create an instance of the RansomPaper
    private void Awake()
    {
        instance = this;
    }

    //close the ransom paper
    public void CloseRansomPaper()
    {
        _ransomPaper.SetActive(false);
    }

    //open the ransom paper
    public void OpenRansomPaper()
    {
        _ransomPaper.SetActive(true);
    }
}
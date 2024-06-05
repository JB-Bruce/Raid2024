using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<string> Tutorials = new List<string>();

    public Transform EnemyTransform;

    public TextMeshProUGUI TextTutorial;
    
    [SerializeField] private int _tutorialincrement = -1;

    /// <summary>
    /// create an instance of the tutorial manager
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        EnemyTransform.gameObject.SetActive(false);

        NextTutorial();
    }

    private void Update()
    {
        //Check if it is in the right moment of the tutorial and if there is a weapon in hte weapon slot
        if (_tutorialincrement == 2 && Inventory.Instance.weaponSlots[0].Item != null)
        {
            NextTutorial();
        }

        if (_tutorialincrement == 3)
        {
            EnemyTransform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    ///     Increment a variable to go to the next step in the tutorial
    /// </summary>
    public void NextTutorial()
    {
        _tutorialincrement++;
        if (_tutorialincrement >= Tutorials.Count)
        {
            TextTutorial.enabled = false;
            return;
        }
            
        TextTutorial.text = Tutorials[_tutorialincrement];

    }

    /// <summary>
    ///     Getter for tutorial increment
    /// </summary>
    public int TutorialIncrement()
    { return _tutorialincrement; }
}

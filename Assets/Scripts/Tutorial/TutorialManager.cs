using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<string> Tutorials = new List<string>();

    public TextMeshProUGUI TextTutorial;
    

    private int _tutorialincrement = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        NextTutorial();
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

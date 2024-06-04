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

    // Start is called before the first frame update
    void Start()
    {
        NextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTutorial()
    {
        _tutorialincrement++;
        if (_tutorialincrement > Tutorials.Count)
        {
            TextTutorial.enabled = false;
            return;
        }
            
        TextTutorial.text = Tutorials[_tutorialincrement];

    }
}

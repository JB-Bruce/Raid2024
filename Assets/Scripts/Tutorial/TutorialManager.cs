using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<string> Tutorials = new List<string>();

    public Transform EnemyTransform;
    public Transform PlayerTransform;
    public Transform CenterAlliesTransform;

    public TextMeshProUGUI TextTutorial;

    public List<GameObject> ListAllies = new List<GameObject>();
    public List<GameObject> ListUIToDeactivate = new List<GameObject>();

    
    
    [SerializeField] private int _tutorialincrement = -1;




    private bool _setEnableToTrue = true;

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

        if (_tutorialincrement == 4 && _setEnableToTrue)
        {
            foreach(GameObject go in ListAllies)
            {
                go.SetActive(true);
            }
            foreach(GameObject go in ListUIToDeactivate)
            {
                go.SetActive(false); 
            }
            MovePlayer movePlayer = PlayerTransform.GetComponent<MovePlayer>();
            Destroy(movePlayer);
            _setEnableToTrue = false;
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

    /// <summary>
    ///     Get the Player Faction from playerPrefs
    /// </summary>
    public Faction GetPlayerFaction()
    {
        Faction playerFaction = Faction.Survivalist;

        switch (PlayerPrefs.GetString("CharacterFaction"))
        {
            case "Military":
                playerFaction = Faction.Military;
                break;

            case "Scientist":
                playerFaction = Faction.Scientist;
                break;

            case "Utopist":
                playerFaction = Faction.Utopist;
                break;

            case "Survivalist":
                playerFaction = Faction.Survivalist;
                break;

            default:
                playerFaction = Faction.Military;
                break;

        }

        return playerFaction;
    }

    
}

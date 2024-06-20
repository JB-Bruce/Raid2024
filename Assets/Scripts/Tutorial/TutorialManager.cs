using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;


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
    
    public List<Transform> ListTutorialTarget = new List<Transform>();

    public Animator FadeInAnimator;
    
    [SerializeField] private int _tutorialincrement = -1;

    private bool _setEnableToTrue = true;
    private bool _playOnce = true;

    private ArrowQuest _arrowQuest;

    [SerializeField]
    private string _sceneToLoad;

    /// <summary>
    /// create an instance of the tutorial manager
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _arrowQuest = ArrowQuest.Instance;
    }

    void Start()
    {
        NextTutorial();
    }

    private void Update()
    {
        if (!CharacterCustomisation.Instance.PlayPressed)
            return;

        if (CharacterCustomisation.Instance.SkipTutorial)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }

        //Take care of everything that appends in the tutorial
        switch (_tutorialincrement)
        {
            case 0:

                ArrowQuest.Instance.QuestTarget = ListTutorialTarget[0];

                break;
                
            case 1:

                ArrowQuest.Instance.QuestTarget = ListTutorialTarget[1];

                break;

            case 2:
                ArrowQuest.Instance.QuestTarget = null;

                if (Inventory.Instance.weaponSlots[0].Item != null)
                {
                    NextTutorial();
                }

                break;

            case 3:

                ArrowQuest.Instance.QuestTarget = ListTutorialTarget[2];

                EnemyTransform.gameObject.SetActive(true);  
                
                break;

            case 4:
                ArrowQuest.Instance.QuestTarget = null;
                if (_setEnableToTrue)
                {
                    foreach (GameObject go in ListAllies)
                    {
                        go.SetActive(true);
                    }
                    foreach (GameObject go in ListUIToDeactivate)
                    {
                        go.SetActive(false);
                    }
                    MovePlayer movePlayer = PlayerTransform.GetComponent<MovePlayer>();
                    Destroy(movePlayer);
                    _setEnableToTrue = false;
                }
                break;

            case 5:
                ArrowQuest.Instance.QuestTarget = null;
                if (_playOnce)
                {
                    FadeInAnimator.Play("FadeIn");
                    StartCoroutine(ChangeSceneToGame());
                    _playOnce = false;
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator ChangeSceneToGame()
    {
        yield return new WaitForSeconds(FadeInAnimator.GetCurrentAnimatorClipInfo(0).Length + 1);
        SceneManager.LoadScene(_sceneToLoad);
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

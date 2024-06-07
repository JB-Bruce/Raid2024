using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AllyTutorial : MonoBehaviour
{
    public List<Sprite> HairsSprites;
    public List<Sprite> BodiesSprites;
    public List<Sprite> LegsSprites;

    public GameObject Hairs;
    public GameObject Bodie;
    public GameObject Legs;

    public Transform DestinationTransform;

    private bool _setPlayerFactionOnce = true;

    private Vector3 _spawnAllyPosition = Vector3.zero;
    private Vector3 _allyDestination = Vector3.zero;

    private Transform _selfTransform;

    private float travelTime = 2.0f;
    private float elapsedTime = 0.0f;

    private TutorialManager _tutorialManager;

    private void Start()
    {
        _selfTransform = transform;
        _spawnAllyPosition = _selfTransform.position;
        _allyDestination = DestinationTransform.position;

        _tutorialManager = TutorialManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_setPlayerFactionOnce && CharacterCustomisation.Instance.PlayPressed)
        {
            ChangeAlliesSpriteFromFaction();
        }

        if (_tutorialManager.TutorialIncrement() == 4)
        {
            elapsedTime += Time.deltaTime;

            elapsedTime = Mathf.Clamp(elapsedTime, 0, travelTime);
            
            float t = elapsedTime / travelTime;

            
            _selfTransform.position = Vector3.Lerp(_spawnAllyPosition, _allyDestination, t);

            if (Vector3.Distance(_selfTransform.position, _allyDestination) < 1.0f)
            {
                _tutorialManager.NextTutorial();
            }

        }
    }

    /// <summary>
    ///   Change the sprite of the allies using an index
    /// </summary>
    private void ChangeSpriteFromIndex(int index)
    {
        Hairs.GetComponent<SpriteRenderer>().sprite = HairsSprites[index];
        Bodie.GetComponent<SpriteRenderer>().sprite = BodiesSprites[index];
        Legs.GetComponent<SpriteRenderer>().sprite = LegsSprites[index];
    }

    /// <summary>
    ///     when the play button is pressed set the right sprite for the allies
    /// </summary>
    private void ChangeAlliesSpriteFromFaction()
    {
        switch (TutorialManager.Instance.GetPlayerFaction())
        {
            case Faction.Military:
                ChangeSpriteFromIndex(0);
                break;

            case Faction.Utopist:
                ChangeSpriteFromIndex(1);
                break;

            case Faction.Survivalist:
                ChangeSpriteFromIndex(2);
                break;

            case Faction.Scientist:
                ChangeSpriteFromIndex(3);
                break;

            default:
                ChangeSpriteFromIndex(0);
                break;
        }
    }
}

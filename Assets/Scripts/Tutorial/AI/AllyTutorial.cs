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

    private bool _setPlayerFactionOnce = true;

    // Update is called once per frame
    void Update()
    {

        // when the play button is pressed set the right sprite for the allies
        if ( _setPlayerFactionOnce) //CharacterCustomisation.Instance.PlayPressed &&
        {
            switch (TutorialManager.Instance.GetPlayerFaction())
            {
                case Faction.Military:
                    ChangeSprite(0);
                    break;

                case Faction.Utopist:
                    ChangeSprite(1);
                    break;

                case Faction.Survivalist:
                    ChangeSprite(2);
                    break;

                case Faction.Scientist:
                    ChangeSprite(3);
                    break;

                default:
                    ChangeSprite(0);
                    break;
            }
        }
    }

    /// <summary>
    ///   Change the sprite of the allies using an index
    /// </summary>
    private void ChangeSprite(int index)
    {
        Hairs.GetComponent<SpriteRenderer>().sprite = HairsSprites[0];
        Bodie.GetComponent<SpriteRenderer>().sprite = BodiesSprites[0];
        Legs.GetComponent<SpriteRenderer>().sprite = LegsSprites[0];
    }
}

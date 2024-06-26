using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FactionLeader))]
[RequireComponent(typeof(FactionUnitManager))]
public class FactionSc : MonoBehaviour
{
    private FactionLeader _factionLeader;
    private FactionUnitManager _unitManager;
    private List<POI> _caughtPOI = new();
    [SerializeField] Transform[] _guardTransform = new Transform[2];

    [Header("PNJ")]
    public PnjFactionTrader _trader;
    public PnjFactionTrader _armory;
    public Pnj _mainQuest;
    public PnjFactionQuestGiver _questGiver;
    public List<Interactable> interactables = new();

    private void Awake()
    {
        _factionLeader = GetComponent<FactionLeader>();
        _unitManager = GetComponent<FactionUnitManager>();
    }

    private void Start()
    {
        StartCoroutine(WinBuildingExperience());
    }

    // Check if the faction is the owner of POI
    public void HaveCaughtPOI(POI _poi, Faction _ownerFaction)
    {
        if(_ownerFaction == _unitManager.faction && !_caughtPOI.Contains(_poi))
        {
            _caughtPOI.Add(_poi);
        }
        else if(_ownerFaction != _unitManager.faction && _caughtPOI.Contains(_poi))
        {
            _caughtPOI.Remove(_poi);
        }
    }

    // Coroutine For add building experience
    private IEnumerator WinBuildingExperience()
    {
        while (true)
        {
            for (int i = 0; i < _caughtPOI.Count; i++)
            {
                _factionLeader.GainBuildingXP(_caughtPOI[i].priority);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    // Set position of guard point
    public void SetGuardPoint(Vector3 pos1, Vector3 pos2)
    {
        _guardTransform[0].localPosition = pos1;
        _guardTransform[1].localPosition = pos2;
    }
    public FactionUnitManager FactionUnitManager => _unitManager;
    public FactionLeader FactionLeader => _factionLeader;
}

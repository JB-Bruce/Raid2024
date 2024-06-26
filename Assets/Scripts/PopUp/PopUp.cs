using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _popUp;
    [SerializeField] private Color _positiveReputation;
    [SerializeField] private Color _negativeReputation;
    [SerializeField] private Faction _faction;

    private void Start()
    {
        FactionManager.Instance.RelationWithPlayerChange.AddListener(SpawnPopUp);
    }
    public void SpawnPopUp(Faction faction, float addReputation)
    {
        if(_faction != faction) 
        {
            return; 
        }

        GameObject go = Instantiate(_popUp, _parent);
        TextMeshProUGUI _textMesh = go.GetComponent<TextMeshProUGUI>();

        if (addReputation > 0) 
        {
            _textMesh.color = _positiveReputation;
            _textMesh.text = "+"+addReputation.ToString();
        }
        else 
        {
            _textMesh.color = _negativeReputation;
            _textMesh.text = addReputation.ToString();
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField]
    private GameObject _player;

    [SerializeField] 
    private FactionUnitManager _unitManager;

    [SerializeField]
    private float radiusAroundPlayer = 50;

    [SerializeField]
    private float radiusAroundPoint = 50;

    [SerializeField]
    private GameObject _objective;

    private List<GameObject> _spawnUnits = new();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start a wave (point  = point to defend // duration = duration of the wave // intensity = number of unit who spawn/second)
    public void StartWave(Vector3 point, float duration, int intencity, float waitSpawnTimer)
    {
        _objective.transform.position = point;
        StartCoroutine(Wave(point, duration, intencity, waitSpawnTimer));
    }

    // Coroutine of the wave
    private IEnumerator Wave(Vector3 point, float duration, int intencity, float waitSpawnTimer)
    {
        float realDuration = Time.time + duration;
        while(Time.time < realDuration) 
        {
            yield return new WaitForSeconds(waitSpawnTimer);
            for (int i = 0; i < intencity; i++)
            {
                _spawnUnits.Add(_unitManager.SpawnWaveUnit(GetRandomSpawnPosition(), _objective.transform.position));
            }
            if(_objective == null)
            {
                break;
            }
        }
        yield return new WaitForSeconds(5);
        EndWave();
        QuestManager.instance.CheckQuestTrigger(QuestManager.QuestTriggerType.defend, "DefendBase");
    }

    // Get a random spawn position for the unit
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 _pos = Vector3.zero;
        do
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle;

            float randomDistance = UnityEngine.Random.Range(radiusAroundPoint, (radiusAroundPoint+10));

            _pos = _objective.transform.position + (randomDirection * randomDistance);

        } while (CantSpawnHere(_pos));

        return _pos;
    }

    // Call when the wave is finish make all the unit go to patrol
    private void EndWave()
    {
        for (int i = 0; i < _spawnUnits.Count; i++)
        {
            if (_spawnUnits[i] != null && _spawnUnits[i].GetComponent<UnitBT>() != null)
            {
                _spawnUnits[i].GetComponent<UnitBT>().order = UnitOrder.Patrol;
            }
        }
    }

    // Check if the unit can't spawn here
    private bool CantSpawnHere(Vector3 position)
    {
        if (Vector3.Distance(position, _player.transform.position) < radiusAroundPlayer || Vector3.Distance(position, _objective.transform.position) < radiusAroundPlayer)
        {
            return true;
        }
        return false;
    }
}

using System.Collections;
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

    private Vector3 _point;

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
        duration += Time.time;
        _point = point;
        StartCoroutine(Wave(point, duration, intencity, waitSpawnTimer));
    }

    // Coroutine of the wave
    private IEnumerator Wave(Vector3 point, float duration, int intencity, float waitSpawnTimer)
    {
        while(Time.time < duration) 
        {
            yield return new WaitForSeconds(waitSpawnTimer);
            for (int i = 0; i < intencity; i++)
            {
                _unitManager.SpawnWaveUnit(GetRandomSpawnPosition(), _point);
            }
        }
        QuestManager.instance.CheckQuestTrigger(QuestManager.QuestTriggerType.defend, "DefendBase");
    }

    // Get a random spawn position for the unit
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 _pos = Vector3.zero;
        do
        {
            Vector3 randomDirection = Random.insideUnitCircle;

            float randomDistance = Random.Range(radiusAroundPoint, (radiusAroundPoint+10));

            _pos = _point + (randomDirection * randomDistance);

        } while (CantSpawnHere(_pos));

        return _pos;
    }

    // Check if the unit can't spawn here
    private bool CantSpawnHere(Vector3 position)
    {
        if (Vector3.Distance(position, _player.transform.position) < radiusAroundPlayer || Vector3.Distance(position, _point) < radiusAroundPlayer)
        {
            return true;
        }
        return false;
    }
}

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class POI : MonoBehaviour
{
    [Header("POI parameters")]
    [Range(-100,100)] public float capturePercentage = 0;
    public Faction ownerFaction;
    public float captureSpeed = 0.1f;
    public List<Humanoid> unitInCaptureZone;
    public float Timer = 0;
    public float captureTime = 1f;
    public int priority = 0;
    private bool _captured = false;

    [Header("Guard POI")]
    public Transform poiPosition;
    public float minDistancePOI = 0;
    public float maxDistancePOI = 0;

    FactionManager _factionManager;

    private void Start()
    {
        _factionManager = FactionManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Timer < Time.time) // Wait captureTime and calculate the new capture percentage
        {
            Timer = Time.time + captureTime;
            POICapture();
        }
    }

    // add the capture pourcentage of all the faction in the POI
    private void POICapture()
    {
        for(int i = 0; i < unitInCaptureZone.Count; i++) 
        {
            AddCapturePercentage(unitInCaptureZone[i].faction);
        }
    }

    // Increase the capture pourcentage for the faction in the POI
    private void AddCapturePercentage(Faction faction)
    {
        if(faction == ownerFaction) 
        {
            capturePercentage += captureSpeed;
            Mathf.Clamp(capturePercentage, -100, 120);

            if(capturePercentage >= 100 && !_captured)
            {
                _captured = true;
                _factionManager.IsPoiCaught.Invoke(this, ownerFaction);
            }
            else if(_captured && capturePercentage < 100)
            {
                _captured = false;
                _factionManager.IsPoiCaught.Invoke(this, Faction.Null);
            }
        }
        else if(_factionManager.GetReputation(ownerFaction, faction) >= _factionManager.allyReputation && capturePercentage >= 100)
        {
            return;
        }
        else
        {
            capturePercentage -= captureSpeed;
        }

        if(capturePercentage < 0) 
        {
            capturePercentage = Mathf.Abs(capturePercentage);
            ownerFaction = faction;
        }

    }

    private void 
        ter2D(Collider2D collision)
    {
        if(collision.transform.parent.parent == null) 
        {
            return;
        }
        if (collision.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid) && !unitInCaptureZone.Contains(humanoid) && !collision.isTrigger)
        {
            unitInCaptureZone.Add(humanoid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.parent == null)
        {
            return;
        }
        if (collision.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid) && !collision.isTrigger)
        {
            unitInCaptureZone.Remove(humanoid);
        }

    }
}

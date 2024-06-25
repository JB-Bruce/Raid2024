using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorManager : MonoBehaviour
{
    [System.Serializable]
    public struct DecorType
    {
        public GameObject prefab;
        [Range(0, 1)] public float percentage;
        public float minSize;
        public float maxSize;
    }

    [Header("Decor Types")]
    [SerializeField] DecorType[] _decorTypes;

    [Header("Generation Settings")]
    [SerializeField] int _numberOfDecor = 100;
    [SerializeField] float _minDistanceBetweenDecor = 0.5f;
    [SerializeField] float _maxDistanceBetweenDecor = 2f;
    [SerializeField] float _minOffsetDistance = 0.5f;
    [SerializeField] float _maxOffsetDistance = 2f;
    [SerializeField] int _maxAttempts = 10;

    [Header("Generation Bounds")]
    [SerializeField] List<GameObject> _boundaryPoints;

    private List<Vector2> _polygonPoints;

    private void Start()
    {
        _polygonPoints = new List<Vector2>();
        foreach (var pointObj in _boundaryPoints)
        {
            _polygonPoints.Add(pointObj.transform.position);
        }

        GenerateDecor();
    }

    private void GenerateDecor()
    {
        float totalPercentage = 0f;

        foreach (DecorType decorType in _decorTypes)
        {
            totalPercentage += decorType.percentage;
        }

        if (totalPercentage != 1f)
        {
            Debug.LogError("Total decor percentages must equal 1.");
            return;
        }

        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < _numberOfDecor; i++)
        {
            float randomValue = Random.value;
            GameObject selectedPrefab = null;
            float cumulative = 0f;
            float scaleFactor = 1f;

            foreach (DecorType decorType in _decorTypes)
            {
                cumulative += decorType.percentage;
                if (randomValue <= cumulative)
                {
                    selectedPrefab = decorType.prefab;
                    scaleFactor = Random.Range(decorType.minSize, decorType.maxSize);
                    break;
                }
            }

            if (selectedPrefab == null)
            {
                Debug.LogError("Decor prefab not assigned correctly.");
                return;
            }

            Vector2 randomPos = Vector2.zero;
            bool positionValid = false;

            for (int attempt = 0; attempt < _maxAttempts; attempt++)
            {
                randomPos = GetRandomPointInPolygon(_polygonPoints);

                // Add a random offset to make positions more diverse
                float randomOffset = Random.Range(_minOffsetDistance, _maxOffsetDistance);
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector2 offsetPosition = randomPos + randomDirection * randomOffset;

                // Ensure the offset position is within the polygon
                if (!IsPointInPolygon(offsetPosition, _polygonPoints))
                {
                    continue;
                }

                positionValid = true;

                foreach (Vector2 pos in positions)
                {
                    float distance = Vector2.Distance(offsetPosition, pos);
                    if (distance < _minDistanceBetweenDecor || distance > _maxDistanceBetweenDecor)
                    {
                        positionValid = false;
                        break;
                    }
                }

                if (positionValid)
                {
                    randomPos = offsetPosition;
                    break;
                }
            }

            if (positionValid)
            {
                positions.Add(randomPos);
                GameObject decorInstance = Instantiate(selectedPrefab, new Vector3(randomPos.x, randomPos.y, 0f), Quaternion.identity);
                decorInstance.transform.SetParent(transform);
                decorInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
        }
    }

    Vector2 GetRandomPointInPolygon(List<Vector2> polygon)
    {
        // Get the bounding box of the polygon
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (Vector2 point in polygon)
        {
            if (point.x < minX) minX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.x > maxX) maxX = point.x;
            if (point.y > maxY) maxY = point.y;
        }

        Vector2 randomPoint;
        do
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            randomPoint = new Vector2(x, y);
        }
        while (!IsPointInPolygon(randomPoint, polygon));

        return randomPoint;
    }

    bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool isInside = false;
        int j = polygon.Count - 1;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (polygon[i].y < point.y && polygon[j].y >= point.y || polygon[j].y < point.y && polygon[i].y >= point.y)
            {
                if (polygon[i].x + (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < point.x)
                {
                    isInside = !isInside;
                }
            }
            j = i;
        }
        return isInside;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ForestManager : MonoBehaviour
{

    public List<Transform> ForestTrees = new List<Transform>(); 

    public List<Tree> Prefabs = new List<Tree>();

    private float _minSizetree = 0.75f;
    private float _maxSizetree = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        RandomTree();
    }



    //Random the size and the sprite of the the tree in the list
    private void RandomTree()
    {

        foreach (Transform t in transform)
        {
            ForestTrees.Add(t);
            float size = Random.Range(_minSizetree, _maxSizetree + 0.01f);
            float randompercentage = Random.Range(0, 100.0f);
            float prefabspercentage = 0;
            float lastpercentage = 0;


            foreach (Tree tree in Prefabs)
            {
                lastpercentage = prefabspercentage;
                prefabspercentage += tree.Percentage;
                if (prefabspercentage >= randompercentage && randompercentage >= lastpercentage)
                {
                    t.GetComponent<SpriteRenderer>().sprite = tree.SpriteTree;
                }
            }

            t.transform.localScale = new Vector3(size, size, size);
        }
    }


    //contain the sprite and the percentage of a tree
    [System.Serializable]
    public struct Tree
    {
        public Sprite SpriteTree;
        public float Percentage;
    }
}

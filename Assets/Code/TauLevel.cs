using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
     _
  _.;_'-._
 {`--.-'_,}
{; \,__.-'/}
{.'-`._;-';
 `'--._.-'
    .-\\,-"-.
    `- \( '-. \
        \;---,/
    .-""-;\
   /  .-' )\   
   \,---'` \\
            \|
*/

public struct TerrainData
{
    public TerrainType mType;
    public float x;
    public float y;
}

public class TauLevelData
{
    public List<TerrainData> mData;

    public TauLevelData(string filename)
    {
        mData = new List<TerrainData>();
        for(int i=0; i<100; ++i)
        {
            float newX = (i*0.3f)-15f;
            float newY = Random.Range(-1,-3);
            mData.Add(new TerrainData{ mType = TerrainType.FLOOR_SQUARE, x=newX, y=newY} );
        }
    }
}


public class TauLevel : MonoBehaviour 
{
    public GameObject levelActive;
    public GameObject levelInactive;
    
	public GameObject sPrototypeFloorSquare;
	
    
    public Dictionary<TerrainType, List<TauTerrain>> mLevelDictionary;
    public Dictionary<TerrainType, Queue<TauTerrain>> mLevelPool;

    public TauLevelData levelData;

    public bool isLoaded = false; 
    public bool isInit = false; 
    

    public void Awake()
    {
        mLevelDictionary = new Dictionary<TerrainType, List<TauTerrain>>();
        mLevelPool = new Dictionary<TerrainType, Queue<TauTerrain>>();
        mLevelDictionary.Add(TerrainType.FLOOR_SQUARE, new List<TauTerrain>());
        mLevelPool.Add(TerrainType.FLOOR_SQUARE, new Queue<TauTerrain>());
        MakeTerrainPool();
    }

    public void LoadLevel()
    {
        levelData = new TauLevelData("LVL1");
        isLoaded = true;
    }

    public void MakeLevel()
    {
        foreach(TerrainData data in levelData.mData)
        {
            TauTerrain toAdd = null;;
            switch(data.mType)
            {
                case TerrainType.FLOOR_SQUARE: toAdd = GetNextFloorSquare(); break;
            }
            if (toAdd != null)
            {
                Vector3 newPos = new Vector3(data.x, data.y, 0);
                toAdd.transform.position = newPos;
            }
            toAdd.SetActive(true);
        }
        isInit = true;
    }


    public void MakeTerrainPool()
    {
        int lFloorSquareCount = 500;
        for(int i = 0; i < lFloorSquareCount; ++i)
        {
            GameObject toAdd = Instantiate(sPrototypeFloorSquare, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            toAdd.transform.parent = levelInactive.transform;
            toAdd.SetActive(false);
            TauTerrain toAddTerrain = toAdd.GetComponent<TauTerrain>();
            toAddTerrain.mType = TerrainType.FLOOR_SQUARE;
            mLevelPool[TerrainType.FLOOR_SQUARE].Enqueue(toAddTerrain); 
        }
    }

    public TauTerrain GetNextFloorSquare()
    {
        TauTerrain toAdd = mLevelPool[TerrainType.FLOOR_SQUARE].Dequeue();
        mLevelDictionary[TerrainType.FLOOR_SQUARE].Add(toAdd);
        toAdd.transform.parent = levelActive.transform;
        return toAdd;
    }



}

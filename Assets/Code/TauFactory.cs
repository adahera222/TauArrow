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

public class TauFactory : MonoBehaviour 
{
    public GameObject allActiveChunks;
    public GameObject allInactiveChunks;
    public GameObject allActors;
    public GameObject allObjects;

    public GameObject sPrototypeChunk;
    public GameObject sPrototypeHero;
    public GameObject sPrototypeArrow;
    public GameObject sPrototypeBaddie;


    public List<TauChunk> mChunkList;
    public int mLastChunkIndex;
    public int mChunkCount;


    public void Awake()
    {
        MakeChunks();
    }


    public void MakeChunks()
    {
        mChunkCount = 1500;
        for(int i = 0; i < mChunkCount; ++i)
        {
            GameObject toAdd = Instantiate(sPrototypeChunk, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            toAdd.transform.parent = allInactiveChunks.transform;
            toAdd.SetActive(false);
            TauChunk toAddChunk = toAdd.GetComponent<TauChunk>();
            toAddChunk.InitStart();
            mChunkList.Add(toAddChunk); 
        }
        mLastChunkIndex = 0;
    }

    // Note, this can reuse old TauChunks
    public TauChunk GetNextChunk()
    {
        int idx = mLastChunkIndex;
        mLastChunkIndex = (mLastChunkIndex < mChunkCount-1) ? mLastChunkIndex + 1 : 0;
        TauChunk toAdd = mChunkList[idx];
        toAdd.transform.parent = allActiveChunks.transform;
        return toAdd;
    }

    public TauActor GetNextHero()
    {
        GameObject toAdd = Instantiate(sPrototypeHero, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        TauActor toAddActor = toAdd.GetComponent<TauActor>();
        toAddActor.isHuman = true;
        
        return toAddActor;
    }

    public TauObject GetNextArrow()
    {
        GameObject toAdd = Instantiate(sPrototypeArrow, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        TauObject toAddObject = toAdd.GetComponent<TauObject>();       
        return toAddObject;
    }

    public TauActor GetNextBaddie()
    {
        GameObject toAdd = Instantiate(sPrototypeBaddie, new Vector3(0,0,0), Quaternion.identity) as GameObject;

        TauActor toAddActor = toAdd.GetComponent<TauActor>();
        toAddActor.isHuman = false;
        return toAddActor;
    }



}

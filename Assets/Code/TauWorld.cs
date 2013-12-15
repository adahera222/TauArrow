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

public class TauWorld : MonoBehaviour 
{
    public static TauWorld instance;
    public static TauWorld Instance { get { return instance; } }


    public TauFactory factory;
    
    public List<TauActor> actorList = new List<TauActor>();
    public List<TauObject> arrowList = new List<TauObject>();

    public TauWorld()
    {
        instance = this;
    }


    public TauObject CreateArrow()
    {
        TauObject arrow = factory.GetNextArrow();
        arrowList.Add(arrow);
        arrow.InitStart();
        return arrow;
    } 

    public TauActor CreateHero()
    {
        TauActor hero = factory.GetNextHero();
        actorList.Add(hero);
        hero.gameObject.transform.position = new Vector3(5f,1f,0f);
        hero.InitStart();
        return hero;
    }

    public TauActor CreateBaddie()
    {
        TauActor baddie = factory.GetNextBaddie();
        actorList.Add(baddie);
        baddie.gameObject.transform.position = new Vector3(-5f,1f,0f);
        baddie.InitStart();
        return baddie;
    } 
    
    public TauActor FindActor(TauActor src)
    {
        float closestSqDist = 999999;
        TauActor closest = null;
        for(int i=0; i<actorList.Count; ++i)
        {
            TauActor iter = actorList[i];
            if (iter == src)
            {
                continue;
            }
            float sqDist = Utilities.DistanceSquared(src, iter);
            if (sqDist < closestSqDist)
            {
                closestSqDist = sqDist;
                closest = iter;
            }
        } 
        return closest;
    }

    public TauObject FindArrow(TauObject src)
    {
        float closestSqDist = 999999;
        TauObject closest = null;
        for(int i=0; i<arrowList.Count; ++i)
        {
            TauObject iter = arrowList[i];
            if (iter == src)
            {
                continue;
            }
            float sqDist = Utilities.DistanceSquared(src, iter);
            if (sqDist < closestSqDist)
            {
                closestSqDist = sqDist;
                closest = iter;
            }
        } 
        return closest;
    }
}

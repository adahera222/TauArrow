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
    public List<TauObject> knifeList = new List<TauObject>();

    public int ActorCount { get { return actorList.Count; } }

    public TauWorld()
    {
        instance = this;
    }


    public TauObject CreateArrow()
    {
        TauObject arrow = factory.GetNextArrow();
        arrowList.Add(arrow);
        arrow.InitStart();
        arrow.InitFinish();
        return arrow;
    } 

    public TauObject CreateKnife()
    {
        TauObject knife = factory.GetNextKnife();
        knife.scale = 2.5f;
        knifeList.Add(knife);
        knife.InitStart();
        knife.InitFinish();
        return knife;
    } 

    public TauActor CreateHero()
    {
        TauActor hero = factory.GetNextHero();
        actorList.Add(hero);
        hero.gameObject.transform.position = new Vector3(0f,1f,0f);
        hero.InitStart();
        hero.controller.cData = Globals.HeroCData;
        hero.aData = Globals.HeroAData;
        hero.InitFinish();
        return hero;
    }

    public TauActor CreateBaddie()
    {
        TauActor baddie = factory.GetNextBaddie();
        actorList.Add(baddie);
        float startX = (UnityEngine.Random.Range(0,2) == 0) ? -5f : 5f;
        baddie.gameObject.transform.position = new Vector3(startX,1f,0f);
        baddie.InitStart();
        switch(UnityEngine.Random.Range(0,4))
        {
            default:
            case 0: 
                baddie.controller.cData = Globals.GhostCData; 
                baddie.aData = Globals.GhostAData;
                break;
            case 1: 
                baddie.controller.cData = Globals.BugCData; 
                baddie.aData = Globals.BugAData;
                break;
            case 2: 
                baddie.controller.cData = Globals.SneakCData; 
                baddie.aData = Globals.SneakAData;
                break;
            case 3: 
                baddie.controller.cData = Globals.AlienCData; 
                baddie.aData = Globals.AlienAData;
                break;
        }
        baddie.InitFinish();
        
       

        return baddie;
    } 

    public void Kill(TauObject obj)
    {
        
        TauActor actor = obj as TauActor;
        if (actor != null)
        {
            actorList.Remove(actor);
            actor.isAlive = false;
        }
        else
        {
            if (obj.gameObject.tag == Globals.ARROW)
            {
                arrowList.Remove(obj);
            }
            else if (obj.gameObject.tag == Globals.KNIFE)
            {
                knifeList.Remove(obj);
            }
        }
        //obj.rigidbody2D.enabled = false;
        obj.collider2D.enabled = false;
        obj.enabled = false;
        StartCoroutine(FinishKill(obj));
        
    }


    IEnumerator FinishKill(TauObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.gameObject.SetActive(false);
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

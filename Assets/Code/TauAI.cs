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

public class TauAI_Stimulus
{
	public Vector2 curPos;
	public TauActor target;
	public Vector2 targetPos;
}

public class TauAI_Decision
{
	public Vector2 desiredMovePos;
	public Vector2 desiredShootPos;
	public bool shouldShoot;
	public bool shouldJump;
	public float jumpDuration = -1f;
	public float shootDuration = -1f;
}

public abstract class TauAI_Base
{
	protected TauAI_Decision decision;
	
	public abstract TauAI_Decision Think(TauAI_Stimulus stimulus);
	public abstract void Update(float deltaTime);
}            

public class TauAI_Wander : TauAI_Base
{
	public TauAI_Wander()
	{
		decision = new TauAI_Decision();
	}

	public override void Update(float deltaTime)
	{
		if (decision.shootDuration > 0f) { decision.shootDuration -= deltaTime; }
		if (decision.jumpDuration > 0f) { decision.jumpDuration -= deltaTime; }
	}

	public override TauAI_Decision Think(TauAI_Stimulus stimulus)
	{
		Vector2 delta = stimulus.targetPos - stimulus.curPos;
		bool moveTowards = delta.sqrMagnitude > 20f;
		decision.desiredShootPos = stimulus.targetPos;
		if (moveTowards)
		{
			decision.desiredMovePos = stimulus.targetPos;
			//Debug.Log("closer");
		}
		else
		{
			decision.desiredMovePos = stimulus.curPos - delta;	
			//Debug.Log("away");
		}
		if (decision.jumpDuration <= 0f)
		{
			decision.shouldJump = UnityEngine.Random.Range(0,10) > 6;
			decision.jumpDuration = 2f;
			//Debug.Log("trytojump");
		}
		if (moveTowards && decision.shootDuration <= 0f)
		{
			decision.shouldShoot = UnityEngine.Random.Range(0,10) > 2;
			decision.shootDuration = 1.5f;
			//Debug.Log("trytoshoot");
		}
		
		return decision;
	}
}


public class TauAI : MonoBehaviour
{
	private TauActor actor;
	private TauController controller;
	private TauAI_Base state;
	private TauAI_Stimulus stimulus;
	private TauAI_Decision decision;

	public float changeDuration = -1f;
	public float scanDuration = -1f;
	public bool isInit = false;

	public bool CanChange { get { return changeDuration < 0f; } }
	public bool CanScan { get { return scanDuration < 0f; } }

	public void InitStart(TauActor p_actor, TauController p_controller)
	{
		actor = p_actor;
		controller = p_controller;
		stimulus = new TauAI_Stimulus();
		state = new TauAI_Wander();
		InitFinish();
	}

	public void InitFinish()
	{
		isInit = true;
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if (changeDuration > 0f) { changeDuration -= deltaTime; }
		if (scanDuration > 0f) { scanDuration -= deltaTime; }
		if (CanChange)
		{
			changeDuration = 1f;
			DecideChange();
		}
		if (CanScan)
		{
			scanDuration = 0.2f;
			Scan(ref stimulus);
		}
		state.Update(deltaTime);
		decision = state.Think(stimulus);
		React(decision);


	}


	public void DecideChange()
	{
		//state = new TauAI_Wander();
	}

	public void Scan(ref TauAI_Stimulus stimulus)
	{
		stimulus.curPos = new Vector2(controller.posX, controller.posY);
		stimulus.target = TauWorld.Instance.FindActor(actor);
		stimulus.targetPos = new Vector2(stimulus.target.controller.posX, stimulus.target.controller.posY);
	}

	public void React(TauAI_Decision decision)
	{
		float axisX = 0f;
		float axisY = 0f;
		float deltaX = decision.desiredMovePos.x - controller.posX;
		float deltaY = decision.desiredMovePos.y - controller.posY;
		axisX = Mathf.Clamp(deltaX, -1f, 1f);
		//axisY = Mathf.Clamp(deltaY, -1f, 1f);
		axisY = decision.shouldJump ? 1f : 0f;
		controller.HandleAxis(axisX, axisY);
		
		float aimX = decision.desiredShootPos.x - controller.posX;
		float aimY = decision.desiredShootPos.y - controller.posY;
		controller.SetAim(aimX, aimY);


		if (decision.shouldShoot)
		{
			controller.HandleButton(controller.shootButton, true);
			controller.HandleButton(controller.shootButton, false);
		}

	}

}

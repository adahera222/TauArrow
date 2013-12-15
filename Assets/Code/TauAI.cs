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
	public Vector2 desiredPos;
	public bool shouldShoot;
	public bool shouldJump;
}

public abstract class TauAI_Base
{
	protected TauAI_Decision decision;
	
	public abstract TauAI_Decision Think(TauAI_Stimulus stimulus);
}            

public class TauAI_Wander : TauAI_Base
{
	public TauAI_Wander()
	{
		decision = new TauAI_Decision();
	}
	public override TauAI_Decision Think(TauAI_Stimulus stimulus)
	{
		decision.desiredPos = stimulus.targetPos;
		decision.shouldShoot = true;
		decision.shouldJump = false;
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
		stimulus.target = TauDirector.Instance.FindActor(actor);
		stimulus.targetPos = new Vector2(stimulus.target.controller.posX, stimulus.target.controller.posY);
	}

	public void React(TauAI_Decision decision)
	{
		float axisX = 0f;
		float axisY = 0f;
		float deltaX = decision.desiredPos.x - controller.posX;
		float deltaY = decision.desiredPos.y - controller.posY;
		axisX = Mathf.Clamp(deltaX, -1f, 1f);
		//axisY = Mathf.Clamp(deltaY, -1f, 1f);
		axisY = decision.shouldJump ? 1f : 0f;
		controller.HandleAxis(axisX, axisY);
		
		controller.SetAim(deltaX, deltaY);


		if (decision.shouldShoot)
		{
			controller.HandleButton(InputButtonType.MOUSE_LEFT, false);
		}

	}

}

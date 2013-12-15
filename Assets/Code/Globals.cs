using UnityEngine;

public class Globals
{
	public static string TERRAIN = "TERRAIN";
	public static ControllerData Hero;
	public static Vector2 ARROW_POS_DOWN = new Vector2(0.12f, -0.05f);
	public static float ARROW_ROT_DOWN = -30f;
	public static Vector2 ARROW_POS_UP = new Vector2(0.18f, 0.285f);
	public static float ARROW_ROT_UP = 30f;
	public static Vector2 ARROW_POS_STRAIGHT = new Vector2(0.24f, 0.20f);
	public static float ARROW_ROT_STRAIGHT = 0f;

	public static void Load()
	{
		Hero = new ControllerData();
		Hero.normalJumpForce = 200f;
		Hero.crouchJumpForce = 300f;
		Hero.runForce = 10f;
		Hero.flyForce = 2f;
		Hero.jumpDuration = 0.5f;
		Hero.landDuration = 0.2f;
		Hero.crouchDuration = 0.1f;
		Hero.chargeDuration = 1f;


	}
}
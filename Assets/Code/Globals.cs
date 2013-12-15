using UnityEngine;

public class Globals
{
	public static string TERRAIN = "TERRAIN";
	public static ControllerData HeroCData;
	public static ActorData HeroAData;
	public static ControllerData BaddieCData;
	public static ActorData BaddieAData;
	public static Vector2 ARROW_POS_DOWN = new Vector2(0.12f, -0.05f);
	public static float ARROW_ROT_DOWN = -30f;
	public static Vector2 ARROW_POS_UP = new Vector2(0.18f, 0.285f);
	public static float ARROW_ROT_UP = 30f;
	public static Vector2 ARROW_POS_STRAIGHT = new Vector2(0.24f, 0.20f);
	public static float ARROW_ROT_STRAIGHT = 0f;

	public static void Load()
	{
		HeroCData = new ControllerData();
		HeroCData.normalJumpForce = 250f;
		HeroCData.crouchJumpForce = 350f;
		HeroCData.runForce = 10f;
		HeroCData.flyForce = 2f;
		HeroCData.maxRunVelocity = 2f;
		HeroCData.jumpDuration = 0.5f;
		HeroCData.landDuration = 0.2f;
		HeroCData.crouchDuration = 0.3f;
		HeroCData.chargeDuration = 0.2f;
		HeroCData.reloadDuration = -1f;

		HeroAData = new ActorData();
		HeroAData.HP = 20;
		HeroAData.weapon = 1;


		BaddieCData = new ControllerData();
		BaddieCData.normalJumpForce = 200f;
		BaddieCData.crouchJumpForce = 300f;
		BaddieCData.runForce = 10f;
		BaddieCData.flyForce = 2f;
		BaddieCData.maxRunVelocity = 1f;
		BaddieCData.jumpDuration = 0.5f;
		BaddieCData.landDuration = 0.2f;
		BaddieCData.crouchDuration = 0.3f;
		BaddieCData.chargeDuration = 0.3f;
		BaddieCData.reloadDuration = 2f;

		BaddieAData = new ActorData();
		BaddieAData.HP = 10;
		BaddieAData.weapon = 2;

	}
}
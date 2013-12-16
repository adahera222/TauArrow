using UnityEngine;

public class Globals
{
	public static string TERRAIN = "TERRAIN";
	public static string ARROW = "ARROW";
	public static string KNIFE = "KNIFE";
	public static string ACTOR = "ACTOR";
	public static ControllerData HeroCData;
	public static ActorData HeroAData;
	public static ControllerData BaddieCData;
	public static ActorData BaddieAData;
	public static ControllerData GhostCData;
	public static ActorData GhostAData;
	public static ControllerData BugCData;
	public static ActorData BugAData;
	public static ControllerData SneakCData;
	public static ActorData SneakAData;
	public static ControllerData AlienCData;
	public static ActorData AlienAData;
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
		HeroCData.painDuration = -1f;

		HeroAData = new ActorData();
		HeroAData.HP = 40;
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
		BaddieCData.painDuration = 2f;

		BaddieAData = new ActorData();
		BaddieAData.HP = 20;
		BaddieAData.weapon = 2;

		GhostCData = new ControllerData();
		GhostCData.normalJumpForce = 2f*BaddieCData.normalJumpForce;
		GhostCData.crouchJumpForce = 2f*BaddieCData.crouchJumpForce;
		GhostCData.runForce = 0.5f*BaddieCData.runForce;
		GhostCData.flyForce = BaddieCData.flyForce;
		GhostCData.maxRunVelocity = 0.5f*BaddieCData.maxRunVelocity;
		GhostCData.jumpDuration = BaddieCData.jumpDuration;
		GhostCData.landDuration = BaddieCData.landDuration;
		GhostCData.crouchDuration = BaddieCData.crouchDuration;
		GhostCData.chargeDuration = BaddieCData.chargeDuration;
		GhostCData.reloadDuration = BaddieCData.reloadDuration;
		GhostCData.painDuration = BaddieCData.painDuration;

		GhostAData = new ActorData();
		GhostAData.HP = (int)(0.5f*BaddieAData.HP);
		GhostAData.weapon = BaddieAData.weapon;
		GhostAData.mainSprite = SpriteFace.GHOST;
		GhostAData.painSprite = SpriteFace.GHOST_PAIN;

		BugCData = new ControllerData();
		BugCData.normalJumpForce = 0.5f*BaddieCData.normalJumpForce;
		BugCData.crouchJumpForce = 0.5f*BaddieCData.crouchJumpForce;
		BugCData.runForce = BaddieCData.runForce;
		BugCData.flyForce = BaddieCData.flyForce;
		BugCData.maxRunVelocity = BaddieCData.maxRunVelocity;
		BugCData.jumpDuration = BaddieCData.jumpDuration;
		BugCData.landDuration = BaddieCData.landDuration;
		BugCData.crouchDuration = BaddieCData.crouchDuration;
		BugCData.chargeDuration = BaddieCData.chargeDuration;
		BugCData.reloadDuration = BaddieCData.reloadDuration;
		BugCData.painDuration = BaddieCData.painDuration;

		BugAData = new ActorData();
		BugAData.HP = (int)(2f*BaddieAData.HP);
		BugAData.weapon = BaddieAData.weapon;
		BugAData.mainSprite = SpriteFace.BUG;
		BugAData.painSprite = SpriteFace.BUG_PAIN;


		SneakCData = new ControllerData();
		SneakCData.normalJumpForce = BaddieCData.normalJumpForce;
		SneakCData.crouchJumpForce = BaddieCData.crouchJumpForce;
		SneakCData.runForce = BaddieCData.runForce;
		SneakCData.flyForce = BaddieCData.flyForce;
		SneakCData.maxRunVelocity = BaddieCData.maxRunVelocity;
		SneakCData.jumpDuration = BaddieCData.jumpDuration;
		SneakCData.landDuration = BaddieCData.landDuration;
		SneakCData.crouchDuration = BaddieCData.crouchDuration;
		SneakCData.chargeDuration = BaddieCData.chargeDuration;
		SneakCData.reloadDuration = 0.33f*BaddieCData.reloadDuration;
		SneakCData.painDuration = BaddieCData.painDuration;

		SneakAData = new ActorData();
		SneakAData.HP = BaddieAData.HP;
		SneakAData.weapon = BaddieAData.weapon;
		SneakAData.mainSprite = SpriteFace.SNEAK;
		SneakAData.painSprite = SpriteFace.SNEAK_PAIN;


		AlienCData = new ControllerData();
		AlienCData.normalJumpForce = 0.25f*BaddieCData.normalJumpForce;
		AlienCData.crouchJumpForce = 0.25f*BaddieCData.crouchJumpForce;
		AlienCData.runForce = BaddieCData.runForce;
		AlienCData.flyForce = BaddieCData.flyForce;
		AlienCData.maxRunVelocity = BaddieCData.maxRunVelocity;
		AlienCData.jumpDuration = BaddieCData.jumpDuration;
		AlienCData.landDuration = BaddieCData.landDuration;
		AlienCData.crouchDuration = BaddieCData.crouchDuration;
		AlienCData.chargeDuration = BaddieCData.chargeDuration;
		AlienCData.reloadDuration = BaddieCData.reloadDuration;
		AlienCData.painDuration = BaddieCData.painDuration;

		AlienAData = new ActorData();
		AlienAData.HP = (int)(4f*BaddieAData.HP);
		AlienAData.weapon = BaddieAData.weapon;
		AlienAData.mainSprite = SpriteFace.ALIEN;
		AlienAData.painSprite = SpriteFace.ALIEN_PAIN;

	}
}
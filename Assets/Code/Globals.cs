public class Globals
{
	public static string TERRAIN = "TERRAIN";
	public static ControllerData Hero;

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
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
	//ACHIEVEMENTS
	// 1. beat the hard labrynth level without grappling; IMPLEMENTED
	// 2. beat easy without dying; IMPLEMENTED
	// 3. beat medium without dying; IMPLEMENTED
	// 4. beat hard without dying; IMPLEMENTED
	// 5. reach the first checkpoint;
	// 6. enter the wormhole; IMPLEMENTED
	// 7. escape the wormhole; IMPLEMENTED
	// 8. beat easy; IMPLEMENTED
	// 9. beat medium; IMPLEMENTED
	// 10. beat hard; IMPLEMENTED
	// 11. complete standard mode in less than 8 minutes; IMPLEMENTED
	// 12. hang with the fam; IMPLEMENTED
	// 13. go 30 seconds without touching the ground; IMPLEMENTED
	// 14. max out your mouse sensitivity; IMPLEMENTED
	// 15. send me feedback; IMPLEMENTED
	// 16. reach secret ending; IMPLEMENTED
	// 17. beat first pogo level in 0.5 space presses; IMPLEMENTED
	// 18. fall out of the wormhole; IMPLEMENTED

	private string grapplessScene = "Level 210";
	private bool grappled = false;

	private string TJScene = "Level 103";
	private bool jumped = false;

	private WormMove wormMove;
	private CameraFollow cameraFollow;


    // Start is called before the first frame update
    void Start()
    {
		refreshReferences();
	}

    // Update is called once per frame
    void Update()
    {
		refreshReferences();
		checkGrappless();
		checkTJ();

		//Debug.Log(grappled);
    }


	private void checkGrappless()
	{
		if (SceneManager.GetActiveScene().name == grapplessScene)
		{
			if (wormMove.getGrappling())
			{
				grappled = true;
			}

			if (FinishBox.finished && !grappled)
			{
				grappled = true;
				Achieve("ACH_GRAPPLESS");
			}
		}
	}

	private void checkTJ()
	{
		if (SceneManager.GetActiveScene().name == TJScene)
		{
			if (Input.GetKeyDown(KeyCode.Space) && !cameraFollow.getMouseFrozen())
			{
				jumped = true;
			}

			if (FinishBox.finished && !jumped)
			{
				jumped = true;
				Achieve("ACH_TJ");
			}
		}
	}


	private void refreshReferences()
	{
		if (wormMove == null)
		{
			wormMove = FindObjectOfType<WormMove>();
		}
		if (cameraFollow == null)
		{
			cameraFollow = FindObjectOfType<CameraFollow>();
		}
	}

	public static void Achieve(string achievement)
	{
		Debug.LogError("achievement unlocked: " + achievement);

		
		if (!SteamManager.Initialized)
			return;

		SteamUserStats.SetAchievement(achievement);
		SteamUserStats.StoreStats();
		


	}
}

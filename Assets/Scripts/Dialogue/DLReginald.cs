using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue))]
public class DLReginald : MonoBehaviour
{
	private GooseChooserMenu gcm;
	private Dialogue dl;
	private string[] script;

	private int stage = -2;
	private int mald = 2;
    // Start is called before the first frame update
    void Start()
    {
		dl = GetComponent<Dialogue>();
		gcm = FindObjectOfType<GooseChooserMenu>();
	}

    // Update is called once per frame
    void Update()
    {
		switch (stageTransition())
		{
			
			case 0:
				script = new string[4];
				script[0] = "Hello, and welcome to Goose Ranch! I'm Reginald, owner of Goose Ranch!";
				script[1] = "Thank you for your dedication to the game. Making it here is not easy, and should not go unrewarded.";
				script[2] = "As a token of my gratitude, I will name the next hatched goose after you!";
				script[3] = "Just fill out the following information, and you will forever be immortalized as a virtual goose!";
				dl.setText(script, true, false);
				break;
			case 1:
				script = new string[4];
				script[0] = "Your goose request has been successfully sent!";
				script[1] = "If your goose doesn't show up immediately, dont be alarmed. Gooses take time to hatch you know!";
				script[2] = "If you really feel like there's something wrong, please email me at ReginaldRanch@gmail.com.";
				script[3] = "I'll get back to you as soon as I can.";
				dl.setText(script, true, false);
				break;
			case -1:
				break;
		}
    }

	private int stageTransition() //controls when the stage changes
	{
		switch (stage)
		{
			case -2: //Instantly transitions to stage 0 or 1
				if (PlayerPrefs.HasKey("Submitted") && PlayerPrefs.GetString("Submitted") == "True")
				{
					stage = 1;
					return 1;
				}
				else
				{
					stage = 0;
					return 0;
				}

			case 0: //Transitions to stage 1 submitted.
				if (dl.getRead())
				{
					Debug.Log("this is where i would open the menu");
					gcm.setMenuActive(true);
					return 0;
				}
				else if (PlayerPrefs.HasKey("Submitted") && PlayerPrefs.GetString("Submitted") == "True")
				{
					stage = 1;
					return 1;
				}
				return -1;
			case 1:
				if (dl.getRead())
				{

					return 1;
				}
				return -1;
			default:
				return -1;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue))]
public class DLRuben : MonoBehaviour
{
	private Dialogue dl;
	private string[] script;
	public DialogueTriggerEvent fallTrigger;

	private int stage = -2;
	private int mald = 2;
    // Start is called before the first frame update
    void Start()
    {
		dl = GetComponent<Dialogue>();
	}

    // Update is called once per frame
    void Update()
    {
		switch (stageTransition())
		{
			
			case 0:
				script = new string[5];
				script[0] = "You know what I think...";
				script[1] = "I think you're gonna to try to climb out of here. They all do.";
				script[2] = "I'll save you the time. It's not possible.";
				script[3] = "Once you're down here, you're down here for good.";
				script[4] = "Sorry, I dont make the rules.";
				dl.setText(script, true, true);
				break;
			case 1:
				script = new string[1];
				script[0] = "Sorry, I dont make the rules.";
				dl.setText(script, false, false);
				break;
			case 2:
				script = new string[2];
				script[0] = "I tried to tell you. It's not worth the effort.";
				script[1] = "You'll just keep falling down, and you dont want that, now do you?";
				dl.setText(script, true, true);
				mald++;
				break;
			case 3:
				script = new string[2];
				script[0] = "How many times am I gonna have to teach you this lesson?";
				script[1] = "It's not possible. You cant do it. Nobody can do it.";
				dl.setText(script, true, true);
				mald++;
				break;
			case 4:
				script = new string[5];
				script[0] = "It's NOT possible.";
				script[1] = "How HARD is that to UNDERSTAND?";
				script[2] = "NOT!";
				script[3] = "POSSIBLE!";
				script[4] = "Jeez, some people...";
				dl.setText(script, true, true);
				mald++;
				break;
			case 5:
				script = new string[1];
				script[0] = "Told you so.";
				dl.setText(script, true, true);
				stage = 1;
				break;
			case -1:
				break;
		}
    }

	private int stageTransition() //controls when the stage changes
	{
		switch (stage)
		{
			case -2: //Instantly transitions to stage 0
				stage = 0;
				return 0;

			case 0: //Transitions to stage 1 when the last text has been read. 
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				return -1;
			case 1:
				if (fallTrigger.isTriggered(1)) //Transitions when ruben is malding.
				{
					stage = mald;
					return mald;
				}
				return -1;
			case 2:
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				return -1;
			case 3:
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				return -1;
			case 4:
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				return -1;
			case 5:
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				return -1;
			default:
				return -1;
		}
	}
}

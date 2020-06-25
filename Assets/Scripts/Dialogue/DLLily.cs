using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue))]
public class DLLily : MonoBehaviour
{
	private Dialogue dl;
	private string[] script;
	public DialogueTriggerEvent trigger;

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
				script[0] = "Psst! Up here!";
				script[1] = "That was quite a fall you took earlier! Glad to see you're still in one piece.";
				script[2] = "You're almost back up, just got this last hurdle.";
				script[3] = "It's too high for a normal jump. Try doing a wall jump.";
				script[4] = "If you jump back and forth between these rocks you can get a lot more height!";


				dl.setText(script, true, true);
				break;
			case 1:
				script = new string[2];
				script[0] = "Try jumping back and forth between these rocks.";
				script[1] = "It's a bit tricky so dont worry if you dont get it on your first try.";
				dl.setText(script, false, false);
				break;
			case 2:
				script = new string[5];
				script[0] = "I like to sit hear and just look out over the pit.";
				script[1] = "It's really quite peaceful.";
				script[2] = "Life can get overwhelming, and I just need some time alone with my thoughts...";
				script[3] = "...";
				script[4] = "Oh, sorry for rambling. Haha, I didn't mean to say that. I barely even know you yet.";
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
			case -2: //Instantly transitions to stage 0
				stage = 0;
				return 0;

			case 0: //Transitions to stage 1 when the last text has been read. 
				if (dl.getRead())
				{
					stage = 1;
					return 1;
				}
				else if (trigger.isTriggered(1))
				{
					stage = 2;
					return 2;
				}
				return -1;
			case 1:
				if (trigger.isTriggered(1))
				{
					stage = 2;
					return 2;
				}
				return -1;
			default:
				return -1;
		}
	}
}

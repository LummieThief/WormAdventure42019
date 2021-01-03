using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSkinAlert : MonoBehaviour
{
	public Animator animator;
	public void playAnimation()
	{
		animator.SetTrigger("Play");
	}
}

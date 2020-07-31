using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantProtection : MonoBehaviour
{
	public int singleDigitId;
	public static PersistantProtection instance0;
	public static PersistantProtection instance1;
	public static PersistantProtection instance2;
	public static PersistantProtection instance3;
	public static PersistantProtection instance4;
	public static PersistantProtection instance5;
	public static PersistantProtection instance6;
	public static PersistantProtection instance7;
	public static PersistantProtection instance8;
	public static PersistantProtection instance9;
	// Start is called before the first frame update
	void Awake()
    {
		switch (singleDigitId)
		{
			case 0:
				if (instance0 == null)
				{
					instance0 = this;
					return;
				}
				else
				{
					break;
				}
			case 1:
				if (instance1 == null)
				{
					instance1 = this;
					return;
				}
				else
				{
					break;
				}
			case 2:
				if (instance2 == null)
				{
					instance2 = this;
					return;
				}
				else
				{
					break;
				}
			case 3:
				if (instance3 == null)
				{
					instance3 = this;
					return;
				}
				else
				{
					break;
				}
			case 4:
				if (instance4 == null)
				{
					instance4 = this;
					return;
				}
				else
				{
					break;
				}
			case 5:
				if (instance5 == null)
				{
					instance5 = this;
					return;
				}
				else
				{
					break;
				}
			case 6:
				if (instance6 == null)
				{
					instance6 = this;
					return;
				}
				else
				{
					break;
				}
			case 7:
				if (instance7 == null)
				{
					instance7 = this;
					return;
				}
				else
				{
					break;
				}
			case 8:
				if (instance8 == null)
				{
					instance8 = this;
					return;
				}
				else
				{
					break;
				}
			case 9:
				if (instance9 == null)
				{
					instance9 = this;
					return;
				}
				else
				{
					break;
				}
		}
		Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gifter : MonoBehaviour
{
	public string skin;
	private string[] skinKeys = { "rainbow", "bunny", "frog", "bee", "snowman", "angel", "alien", "angler", "bull", "worm", "goose", "rectangle"};

	private float initialHeight;
	public float height = 0.3f;
	private float timer = 0;
	public float speed = 1f;
	public float rotSpeed1 = 50f;
	public float rotSpeed2 = 10f;
	public float rotSpeed3 = -40f;


	public ParticleSystem explosionParticles;
	public ParticleSystem passiveParticles;

	private bool active = true;
	private int gifterId;
	//private SaveLoad save;
	// Start is called before the first frame update
	void Start()
    {
		initialHeight = transform.position.y;
		for (int i = 0; i < skinKeys.Length; i++)
		{
			if (skin.Equals(skinKeys[i]))
			{
				gifterId = i;
				break;
			}
		}
		SaveLoad sl = FindObjectOfType<SaveLoad>();
		if (sl != null)
		{
			string saveData = sl.getValueOf("Skins");
			if (saveData.IndexOf(gifterId + ".", 0) != -1)
			{
				Debug.Log(skin + " was deleted");
				Destroy(gameObject);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime * speed;
		float displacement = (Mathf.Sin(timer) + 0.5f) * height;
		transform.position = new Vector3(transform.position.x, initialHeight + displacement, transform.position.z);
		transform.Rotate(Vector3.up, rotSpeed1 * Time.deltaTime);
		transform.Rotate(Vector3.right, rotSpeed2 * Time.deltaTime);
		transform.Rotate(Vector3.forward, rotSpeed3 * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (active && other.gameObject.tag == "Player")
		{
			collect();
		}
	}

	private void collect()
	{
		active = false;
		SoundManager sm = FindObjectOfType<SoundManager>();
		GetComponent<MeshRenderer>().enabled = false;
		explosionParticles.Play();
		passiveParticles.Stop();
		passiveParticles.Clear();
		SaveLoad.gifterBuffer += gifterId + ".";

		//Debug.Log("looking for " + skin);

		for (int i = 0; i < skinKeys.Length; i++)
		{
			//Debug.Log(skinKeys[i]);
			if (skin.Equals(skinKeys[i]))
			{
				string unlockedSkins = "00000000000000000000000000000001";
				if (PlayerPrefs.HasKey("skin"))
				{
					unlockedSkins = System.Convert.ToString((int)Mathf.Pow(2, 31) + PlayerPrefs.GetInt("skin"), 2);
				}
				unlockedSkins = reverseString(unlockedSkins);

				string binary = System.Convert.ToString((int)Mathf.Pow(2, i), 2);
				int binaryIndex = binary.Length - 1;

				if (unlockedSkins[binaryIndex].Equals('1'))
				{
					Debug.Log("already unlocked this skin");
					
					//Debug.Log(unlockedSkins);
					//Debug.Log(binary + " " + binaryIndex);
				}
				else
				{
					Debug.Log("Unlocked " + skin);
					int value = (int)Mathf.Pow(2, i);
					if (PlayerPrefs.HasKey("skin"))
					{
						PlayerPrefs.SetInt("skin", PlayerPrefs.GetInt("skin") + value);
					}
					else
					{
						PlayerPrefs.SetInt("skin", value);
					}
					FindObjectOfType<NewSkinAlert>().playAnimation();
				}				
				break;
			}

			sm.playGifter(0.95f);
			
		}


		
		
	}
	private string reverseString(string text)
	{
		char[] cArray = text.ToCharArray();
		string reverse = "";
		for (int i = cArray.Length - 1; i > -1; i--)
		{
			reverse += cArray[i];
		}
		return reverse;
	}
}

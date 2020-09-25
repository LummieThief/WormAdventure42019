using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ParticleSystem))]
public class Dialogue : MonoBehaviour
{
	public bool annoying;
	private bool on;
	private float timer;

	public string[] script;
	private int page;
	private Text bubble;
	private Text nametag;
	public Image bg;
	private Image prompt;
	private int index;
	public float delay;
	private ParticleSystem ps;
	private ParticleSystemRenderer psRend;
	public Color freshColor;
	public Material freshSprite;
	public Color readColor;
	public Material readSprite;
	private SoundManager sm;

	private bool goOn;
	private bool hasFreshText;
	
    // Start is called before the first frame update
    void Start()
    {
		
		if (bg == null)
		{
			bg = GameObject.FindGameObjectWithTag("Text BG").GetComponent<Image>();
		}
		bubble = bg.GetComponentsInChildren<Text>()[0];
		nametag = bg.GetComponentsInChildren<Text>()[1];
		//bg = bubble.GetComponentInParent<Image>();
		prompt = bg.GetComponentsInChildren<Image>()[1];
		
		bg.enabled = false;
		nametag.enabled = false;
		ps = GetComponent<ParticleSystem>();
		psRend = GetComponent<ParticleSystemRenderer>();
		page = -1;

		switchOn(false);
		beenRead(false);
		
    }

    // Update is called once per frame
    void Update()
    {
		if (bg == null)
		{
			bg = GameObject.FindGameObjectWithTag("Text BG").GetComponent<Image>();
			bubble = bg.GetComponentsInChildren<Text>()[0];
			nametag = bg.GetComponentsInChildren<Text>()[1];
			//bg = bubble.GetComponentInParent<Image>();
			prompt = bg.GetComponentsInChildren<Image>()[1];
		}
		if (PauseMenu.isPaused)
		{
			bg.enabled = false;
			nametag.enabled = false;
			prompt.enabled = false;
			bubble.enabled = false;
		}
		else if (on)
		{
			prompt.enabled = true;
			bubble.enabled = true;

			if (annoying && page == -1)
			{
				nextPage();
			}

			if (page >= 0)
			{
				bg.enabled = true;
				nametag.enabled = true;
				

				timer += Time.deltaTime;
				if (timer > delay)
				{
					timer = 0;
					if (index < script[page].Length)
					{
						bubble.text += script[page].Substring(index, 1);
						index++;
					}
				}
				ps.Stop();
				ps.Clear();
			}

			if (Input.GetKeyDown(KeyCode.E) && !SceneManager.GetActiveScene().name.Contains("Level"))
			{
				nextPage();
				if (page != -1)
				{
					prompt.enabled = true;
				}
				
			}
		}
		
    }


	private void OnTriggerEnter(Collider other)
	{
		if (StartMenu.isOpen && !SceneManager.GetActiveScene().name.Contains("Level"))
		{
			Debug.Log("start menu is open still");
			goOn = true;
		}
		else if (other.gameObject.tag == "Head")
		{
			Debug.Log("worm in");
			switchOn(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (goOn && !StartMenu.isOpen && other.gameObject.tag == "Head")
		{
			goOn = false;
			switchOn(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Head")
		{
			switchOn(false);
		}
	}

	private void switchOn(bool o)
	{
		if (o)
		{
			on = true;
			prompt.enabled = true;
			ps.Play();
			
		}
		else
		{
			
			on = false;
			page = -1;
			timer = 0;
			index = 0;
			bubble.text = "";
			bg.enabled = false;
			nametag.enabled = false;
			prompt.enabled = false;
			ps.Stop();
			ps.Clear();

			
		}
	}

	private void nextPage()
	{
		page++;
		bubble.text = "";
		index = 0;
		bg.enabled = true;
		nametag.enabled = true;
		nametag.text = gameObject.name;
		if (page >= script.Length)
		{
			switchOn(false);
			switchOn(true);
			beenRead(true);
			prompt.enabled = false;

			annoying = false;
		}
	}

	private void beenRead(bool read)
	{
		hasFreshText = !read;

		if (hasFreshText)
		{
			var main = ps.main;
			main.startColor = freshColor;
			psRend.material = freshSprite;
			if (ps.isPlaying)
			{
				ps.Stop();
				ps.Clear();
				ps.Play();
			}
		}
		else
		{
			var main = ps.main;
			main.startColor = readColor;
			psRend.material = readSprite;
			if (ps.isPlaying)
			{
				ps.Stop();
				ps.Clear();
				ps.Play();
			}
		}
	}

	public void setText(string[] newText, bool alert, bool annoy)
	{
		if (alert)
		{
			beenRead(false);
		}
		
		script = newText;
		annoying = annoy;
	}

	public bool getRead()
	{
		return !hasFreshText;
	}

}

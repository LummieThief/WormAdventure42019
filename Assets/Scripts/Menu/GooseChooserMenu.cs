using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class GooseChooserMenu : MonoBehaviour
{
	public InputField nameField;
	public InputField phraseField;
	public GameObject submitButton;
	public Toggle confirmToggle;
	public GameObject gooseChooserUI;

	private bool confirmed = false;
	private string gooseName;
	private string goosePhrase;

	public static bool isActive;

	

	public void Back()
	{
		setMenuActive(false);
	}

	public void ConfirmToggle(bool value)
	{
		confirmed = value;
		submitButton.SetActive(value);
	}

	public void NameSubmit(string input)
	{
		gooseName = input;
		Debug.Log(gooseName);
	}

	public void PhraseSubmit(string input)
	{
		goosePhrase = input;
		Debug.Log(goosePhrase);
		Debug.Log(goosePhrase.Substring(0, Mathf.Min(100, goosePhrase.Length)));
		Debug.Log(goosePhrase.Substring(Mathf.Min(100, goosePhrase.Length)));
	}

	public void Submit()
	{
		/*
		string info = "Name: {" + gooseName
					+ "}  Catchphrase: {" + goosePhrase
					+ "}  Log: {" + PlayerPrefs.GetInt("unity.player_session_log")
					+ "}  Path: {" + Application.dataPath
					+ "}";
		*/
		string log = "" + PlayerPrefs.GetInt("unity.player_session_log");
		string path = Application.dataPath;
		if (path.Length > 50)
		{
			path = path.Substring(0, 50);
		}
		//Debug.Log(info);


		/*
		AnalyticsResult ar = Analytics.CustomEvent("Goose Data", new Dictionary<string, object>
		{
			{ "Name / ID", gooseName + "::" + log},
			{ "ID / Phrase", log + "::" + goosePhrase},
			{ "ID / Path", log + "::" + path}
		});
		*/
		AnalyticsResult ar = Analytics.CustomEvent("Testing", new Dictionary<string, object>
		{
			{ "Name / ID", gooseName + "::" + log},
			{ "ID / Phrase1", goosePhrase.Substring(0, Mathf.Min(100, goosePhrase.Length)) + "::" + goosePhrase},
			{ "ID / Phrase2", goosePhrase.Substring(Mathf.Min(100, goosePhrase.Length))+ "::" + goosePhrase},
			{ "ID / Path", log + "::" + path}
		});


		Debug.Log(ar);
		if (ar == AnalyticsResult.Ok)
		{
			PlayerPrefs.SetString("Submitted", "True");
			Back();
		}
		
		
	}

	public void setMenuActive(bool value)
	{
		isActive = value;
		gooseChooserUI.SetActive(value);
		if (value)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			PauseMenu.isPaused = true;
			Time.timeScale = 0;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			PauseMenu.isPaused = false;
			Time.timeScale = 1;
		}
	}
}

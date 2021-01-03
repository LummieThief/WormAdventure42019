using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Steamworks;


public class GooseChooserMenu : MonoBehaviour
{
	public InputField nameField;
	public InputField phraseField;
	public GameObject submitButton;
	public Toggle confirmToggle;
	public GameObject gooseChooserUI;
	public GameObject errorText;

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
	}

	public void PhraseSubmit(string input)
	{
		goosePhrase = input;
	}

	public void Submit()
	{
		errorText.SetActive(true);
		SendEmail();
		errorText.SetActive(false);

		Debug.Log("sent");
		PlayerPrefs.SetString("Submitted", "True");
		Back();
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

	public void SendEmail()
	{

		string address = "reginaldranch@gmail.com";
		string password = "G00$3R4NCH"; 
		//Dear reader,
		//If you data mined the source code somehow, please, please don't do anything malicious with this.
		//I spent a really long time trying to figure out how to make this system completely foolproof
		//but I just couldn't get anything to work. Putting the raw password here was the best I could do,
		//and you now have access to it, and there's nothing I can do about that except ask that you please
		//dont ruin it for the rest of the players. They worked hard to get to goose ranch and had fun (hopefully)
		//playing the game and deserve to be rewarded, and hacking into this email only serves the purpose of
		//making it harder for those players to get recognized. If you were dedicated enough to data mine the 
		//source code you probably enjoyed the game, so I hope that I can trust you :)
		//Sincerely, Joey.
		string log = "";
		
		if (PlayerPrefs.HasKey("unity.player_session_log"))
		{
			 log += PlayerPrefs.GetInt("unity.player_session_log");
		}

		string steamID = "";
		if (SteamManager.Initialized)
		{
			steamID += SteamFriends.GetPersonaName();
			steamID += " (" + SteamUser.GetSteamID() + ")";
		}


		Debug.Log("sending email");
		MailMessage mail = new MailMessage();
		SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
		SmtpServer.Timeout = 10000;
		SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
		SmtpServer.UseDefaultCredentials = false;
		SmtpServer.Port = 587;

		mail.From = new MailAddress(address);
		mail.To.Add(new MailAddress("wa4.help@gmail.com"));

		mail.Subject = "new goose request!";
		mail.Body = "Name: " + gooseName + "\n" + 
			"Phrase: " + goosePhrase + "\n" + 
			"Log: " + log + "\n" + 
			"Steam: " + steamID;

		

		SmtpServer.Credentials = new System.Net.NetworkCredential(address, password) as ICredentialsByHost; 
		SmtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		};

		mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
		SmtpServer.Send(mail);
	}



	private void OldSubmit()
	{
		//This is the old analytics code

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
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaregiverGUIScript : MonoBehaviour {
	bool showCaregiverGUI = false;
	bool showSeniorGUI = true;
	Vector3 mousePosition;
	public GameObject marjatta;
	public GUISkin careGiverSkin;
	public SelectionTool selTool;

	public Texture callButton;
	public Texture textButton;
	public Texture gramps;
	public Texture infoTextBG;
	public Texture callingSpeechBubble;
	public Texture callAnsweredSpeechBubble;
	public Texture textReceivedSpeechBubble;
	public Texture transparentBox;
	public Texture redBox;
	public Texture backButton;
	public Texture sendButton;
	public Texture ownTextMessageBox;
	public Texture theirsTextMessageBox;
	public Texture defaultAvatarTexture;
	public Texture caregiverAvatar1;
	public Texture caregiverAvatar2;

	public enum CaregiverGUIState { NONE=0, MAIN, CALLING, CALLING_ANSWERED, CALL_ENDED, TEXTING, TEXT_SENT_NOT_NOTICED, TEXT_NOTICED }
	public CaregiverGUIState currentState = CaregiverGUIState.NONE;

	string messageField = "";
	string messageFieldSenior = "";

	private ArrayList textMessages;
	private Texture caregiverAvatar;
	private string caregiverName;
	private string caregiver1Name = "ANTTI";
	private string caregiver2Name = "JUHANI";
	private bool hide;

	void Start() {
		textMessages = new ArrayList();

		// Randomize caregiver avatar
		if(Random.Range(0,1) == 0) {
			caregiverAvatar = caregiverAvatar1;
			caregiverName = caregiver1Name;
		} else {
			caregiverAvatar = caregiverAvatar2;
			caregiverName = caregiver2Name;
		}
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.H)) {
			// Hide the left handside UI
			if(!hide)
				hide = true;
			else
				hide = false;
		}
	}
	
	public void showGUI() {
		if(!showCaregiverGUI) {
			showCaregiverGUI = true;
			currentState = CaregiverGUIState.MAIN;
		}
		else {
			showCaregiverGUI = false;
			currentState = CaregiverGUIState.NONE;
		}
		
	}

	void OnGUI() {
		GUI.skin = careGiverSkin;

		if(currentState == CaregiverGUIState.CALLING) {
			// Draw the speech bubble to marjatta when calling
			foreach(GameObject bird in selTool.getSelectionList()) {
				Vector3 pos = Camera.main.WorldToScreenPoint(bird.transform.position);
				GUI.Box (new Rect (pos.x - 60, Screen.height - pos.y - 60, 50, 50), callingSpeechBubble);
			}
		}

		if(currentState == CaregiverGUIState.CALLING_ANSWERED) {
			// Draw the ... speech bubble
			foreach(GameObject bird in selTool.getSelectionList()) {
				Vector3 pos = Camera.main.WorldToScreenPoint(bird.transform.position);
				GUI.Box (new Rect (pos.x - 60, Screen.height - pos.y - 60, 50, 50), callAnsweredSpeechBubble);
			}
		}

		if(currentState == CaregiverGUIState.TEXT_SENT_NOT_NOTICED) {
			// Draw the text message received speech bubble
			foreach(GameObject bird in selTool.getSelectionList()) {
				Vector3 pos = Camera.main.WorldToScreenPoint(bird.transform.position);
				GUI.Box (new Rect (pos.x - 60, Screen.height - pos.y - 60, 50, 50), textReceivedSpeechBubble);
			}
		}

		if(currentState == CaregiverGUIState.TEXT_NOTICED) {
			// Draw the ... speech bubble
			foreach(GameObject bird in selTool.getSelectionList()) {
				Vector3 pos = Camera.main.WorldToScreenPoint(bird.transform.position);
				GUI.Box (new Rect (pos.x - 60, Screen.height - pos.y - 60, 50, 50), callAnsweredSpeechBubble);
			}
		}
		
		float phoneX = 10.0f;

		if(showCaregiverGUI) {
			// The caregiver GUI
			switch(currentState) {
			case CaregiverGUIState.NONE:
				break;
			case CaregiverGUIState.MAIN:
				// Main page
				if(!hide) 
					GUI.BeginGroup(new Rect(phoneX,150,300,500));
				else 
					GUI.BeginGroup(new Rect(10,150,300,500));
				GUI.Box (new Rect (0,0,300,500), "", "CareGiverBox");
				
				// Title
				GUI.Box (new Rect(25,20,250,50), infoTextBG);
				GUI.Label(new Rect (90,35,170,20), "CONTACT SENIOR", "CaregiverGUITitle");
				
				// Information text
				GUI.BeginGroup(new Rect(10,80,280,120));
				GUI.Box (new Rect(0,0,280,120), infoTextBG);


				// Get selected avatar, if none, use the text
				if(selTool.getSelectionList().Count < 1) {
					GUI.Label(new Rect (5,5,270,90), "Select bird(s) to call", "CaregiverCallText");
				} else {
					int division = (270 / (selTool.getSelectionList().Count + 1) + 5);

					//Debug.Log(division);
					int indexer = 1;
					foreach(GameObject bird in selTool.getSelectionList()) {
						if(bird.GetComponent<InformationTextScript>()) {
							//Debug.Log(division + " * " + indexer + " = " + indexer * division);
							int imageDimensions = 50;
							int fixer = imageDimensions / 2 + (indexer * 3);
							if(bird.GetComponent<InformationTextScript>().birdAvatar) {
								GUI.Box (new Rect (indexer * division - fixer, 5, imageDimensions, imageDimensions), 
								         bird.GetComponent<InformationTextScript>().birdAvatar);
							} else {
								GUI.Box (new Rect (indexer * division - fixer, 5, imageDimensions, imageDimensions), 
								         defaultAvatarTexture);
							}
							GUI.Label(new Rect (indexer * division - fixer, imageDimensions + 5, imageDimensions, imageDimensions),
							          bird.name, "CaregiverCallText");
							indexer = indexer + 1;
						}

					}
				}

				GUI.EndGroup ();
				
				// Button group
				GUI.BeginGroup(new Rect(0,370,300,500));
				if(GUI.Button (new Rect (35,0,80,80), callButton)) {
					currentState = CaregiverGUIState.CALLING;
				}
				if(GUI.Button (new Rect (180,0,80,80), textButton)) {
					currentState = CaregiverGUIState.TEXTING;
				}
				
				GUI.EndGroup ();
				
				GUI.EndGroup ();
				break;
			case CaregiverGUIState.CALLING:
			case CaregiverGUIState.CALLING_ANSWERED:
				/// When Call button is pressed
				GUI.BeginGroup(new Rect(phoneX,150,300,500));
				GUI.Box (new Rect (0,0,300,500), "", "CareGiverBox");

				GUI.BeginGroup(new Rect(10,80,300,370));
				GUI.Box (new Rect(0,0,280,120), infoTextBG);



				if(selTool.getSelectionList().Count < 1) {
					selTool.addToSelectionList(marjatta);
				}

				int division2 = (270 / (selTool.getSelectionList().Count + 1) + 5);
				
				//Debug.Log(division);
				int indexer2 = 1;
				foreach(GameObject bird in selTool.getSelectionList()) {
					if(bird.GetComponent<InformationTextScript>()) {
						//Debug.Log(division + " * " + indexer + " = " + indexer * division);
						int imageDimensions = 50;
						int fixer = imageDimensions / 2 + (indexer2 * 3);
						if(bird.GetComponent<InformationTextScript>().birdAvatar) {
							GUI.Box (new Rect (indexer2 * division2 - fixer, 5, imageDimensions, imageDimensions), 
							         bird.GetComponent<InformationTextScript>().birdAvatar);
						} else {
							GUI.Box (new Rect (indexer2 * division2 - fixer, 5, imageDimensions, imageDimensions), 
							         defaultAvatarTexture);
						}
						GUI.Label(new Rect (indexer2 * division2 - fixer, imageDimensions + 5, imageDimensions, imageDimensions),
						          bird.name, "CaregiverCallText");
						indexer2 = indexer2 + 1;
					}
					
				}

				GUI.Label(new Rect (60,120,170,20), "CALLING...", "CaregiverCallText");
				
				GUI.EndGroup();

				if(GUI.Button (new Rect (15,380,270,100), redBox)) {
					currentState = CaregiverGUIState.CALL_ENDED;
				}


				GUI.EndGroup ();

				break;
			case CaregiverGUIState.TEXT_NOTICED:
			case CaregiverGUIState.TEXT_SENT_NOT_NOTICED:
			case CaregiverGUIState.TEXTING:
				// Texting gui for caregiver
				GUI.BeginGroup(new Rect(phoneX,150,300,500));
				GUI.Box (new Rect (0,0,300,500), "", "CareGiverBox");

				// Currently sent / received text messages
				GUI.BeginGroup(new Rect(5,10, 285, 420));
				if(textMessages.Count > 0) {
					int i = 0;
					foreach(string o in textMessages) {
						GUIStyle style = new GUIStyle();
						style.normal.textColor = Color.black;
						if(o.Contains("caregiver_message")) {
							string s = o.Replace("caregiver_message", "");
							GUI.Box (new Rect (0,0 + i,285,100), ownTextMessageBox);
							GUI.Label(new Rect(10,10 + i, 275, 100), s, style );
						} else if (o.Contains("senior_message")) {
							string s = o.Replace("senior_message", "");
							GUI.Box (new Rect (0,0 + i,285,100), theirsTextMessageBox);
							GUI.Label(new Rect(10,10 + i, 275, 100), s, style );
						}

						i = i + 105;
					}
				}
				GUI.EndGroup();

				if(GUI.Button (new Rect (5,440,50,50), backButton)) {
					// Using call ended for needed functionality
					// Meaning that the bird will continue walking after texting stopped
					currentState = CaregiverGUIState.CALL_ENDED;
				}

				if(GUI.Button (new Rect (245,440,50,50), sendButton)) {
					textMessages.Add(messageField + "caregiver_message");
					messageField = "";
					if(selTool.getSelectionList().Count < 1)
						selTool.addToSelectionList(marjatta);
					if(currentState != CaregiverGUIState.TEXT_NOTICED)
						currentState = CaregiverGUIState.TEXT_SENT_NOT_NOTICED;

				}

				messageField = GUI.TextArea(new Rect (55,440,190,50), messageField, "TextMessageField");

				GUI.EndGroup ();
				break;
			default:
				break;
			}
		}
		// The senior GUI
		if(showSeniorGUI) {
			switch(currentState) { 
			case CaregiverGUIState.CALLING_ANSWERED:
				// When the senior answers the phone
				GUI.BeginGroup(new Rect (Screen.width - 310,150,300,500));
				GUI.Box (new Rect (0,0,300,500), "", "CareGiverBox");
				
				GUI.BeginGroup(new Rect (10,80,300,370));
				GUI.Box (new Rect (0,0,280,120), infoTextBG);
				GUI.Box (new Rect (95, 5, 90, 90), caregiverAvatar);


				GUI.EndGroup();

				GUI.Label(new Rect (60,200,170,20), "CAREGIVER " + caregiverName + " IS CALLING", "CaregiverCallText");

				if(GUI.Button (new Rect (15,380,270,100), redBox)) {
					currentState = CaregiverGUIState.CALL_ENDED;
				}

				
				GUI.EndGroup ();

				break;
			case CaregiverGUIState.TEXT_NOTICED:
				// When the senior notices text message
				GUI.BeginGroup(new Rect(Screen.width - 310,150,300,500));
				GUI.Box (new Rect (0,0,300,500), "", "CareGiverBox");
				
				// Currently sent / received text messages
				GUI.BeginGroup(new Rect(5,10, 285, 420));
				if(textMessages.Count > 0) {
					int i = 0;
					foreach(string o in textMessages) {
						GUIStyle style = new GUIStyle();
						style.normal.textColor = Color.black;
						// Show different boxes for sent and received
						if(o.Contains("caregiver_message")) {
							string s = o.Replace("caregiver_message", "");
							GUI.Box (new Rect (0,0 + i,285,100), theirsTextMessageBox);
							GUI.Label(new Rect(10,10 + i, 275, 100), s, style );
						} else if (o.Contains("senior_message")) {
							string s = o.Replace("senior_message", "");
							GUI.Box (new Rect (0,0 + i,285,100), ownTextMessageBox);
							GUI.Label(new Rect(10,10 + i, 275, 100), s, style );
						}
						i = i + 105;
					}
				}
				GUI.EndGroup();
				
				if(GUI.Button (new Rect (5,440,50,50), backButton)) {
					currentState = CaregiverGUIState.CALL_ENDED;
				}
				
				if(GUI.Button (new Rect (245,440,50,50), sendButton)) {
					textMessages.Add(messageFieldSenior + "senior_message");
					messageFieldSenior = "";
					if(currentState != CaregiverGUIState.TEXT_NOTICED)
						currentState = CaregiverGUIState.TEXT_SENT_NOT_NOTICED;
				}
				
				messageFieldSenior = GUI.TextArea(new Rect (55,440,190,50), messageFieldSenior, "TextMessageField");
				
				GUI.EndGroup ();
				break;
			default:
				break;
			}
		}
	}

}

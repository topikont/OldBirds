using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformationTextScript : MonoBehaviour {
	public Texture birdAvatar;

	public string birdName;
	public string birdStatus;
	public string nextEvent;
	public string[] birdInfo;
	public RectTransform informationWindowObject;
	public Canvas UICanvas;

	private RectTransform informationWindowInstance;
	private Text nameText;
	private Text statusText;
	private Text nextEventText;
	private Text infoText;
	
	private bool enabled = false;
	
	void Start () {	
		if(informationWindowObject) {
			informationWindowInstance = Instantiate(informationWindowObject);
			informationWindowInstance.SetParent(UICanvas.transform, false);
			nameText = informationWindowInstance.FindChild("NamePanel").FindChild("NameText").GetComponent<Text>();
			statusText = informationWindowInstance.FindChild("StatusPanel").FindChild("StatusText").GetComponent<Text>();
			nextEventText = informationWindowInstance.FindChild("NextEventPanel").FindChild("EventText").GetComponent<Text>();
			infoText = informationWindowInstance.FindChild("AdditionalInfoPanel").FindChild("InfoText").GetComponent<Text>();
			
			nameText.text = birdName;
			statusText.text = birdStatus;
			nextEventText.text = nextEvent;
			string sInfoText = "";
			foreach(string text in birdInfo) {
				sInfoText = sInfoText + text + "\n";
				
			}
			infoText.text = sInfoText;
			informationWindowInstance.gameObject.SetActive(false);
		}
		
	}
	
	public void show() {
		if(enabled) {
			this.enabled = false;
			disableWindow();
		} else {
			this.enabled = true;
			enableWindow();
		}
		
	}
	
	public void setShow(bool show) {
		this.enabled = show;
		if(show) enableWindow();
		else disableWindow();
	}


	void enableWindow ()
	{
		if(informationWindowInstance)
			informationWindowInstance.gameObject.SetActive(true);
	}

	void disableWindow ()
	{
		if(informationWindowInstance)
			informationWindowInstance.gameObject.SetActive(false);
	}
}

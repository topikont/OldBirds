#pragma strict
import System.IO;
import System.Net;

var sx: double=0;
var sz: double=0;
var sx1: double=0;
var sz1: double=0;
var sx2: double=5;
var sz2: double=5;
var sx3: double=0;
var sz3: double=0;
var sx4: double=0;
var sz4: double=0;
var steta: double=0;
var snewteta: double=0;
var steta1: double=0;
var steta2: double=0;
var scharkheshA: double=1;
var sharekatA: double=0;
var sentezarA: double=0;
var stxtA="salam";
var stxtB="salam";
var stempA="salam";
var stempB="salam";
var skama: int=0;
var srotationy: double=0;

var executingCorouti = false;
var updateStart=false;
var matn="nothing";

var Target : GameObject; 

var Stockmanns : GameObject[]; 
var Stockmann : GameObject; 
var Indoors : GameObject[]; 
var Indoor : GameObject;
var Outdoors : GameObject[]; 
var Outdoor : GameObject; 

var WhereIsAvatar="indoor";
var IndoorStartUpdate=false;
var OutsideStartUpdate=false;
var StockmannStartUpdate=false;

function Start () { 
	
	
	
}

function Update () {


	if(executingCorouti == false){
	StartCoroutine(Mycoroutine());
	}
	
	if(scharkheshA==1)
	{
	scharkhesh ();
	}
	
	if(sharekatA==1)
	{
	sharekat ();
	}
	
	if(sentezarA==1)
	{
	sentezar ();
	}
	
	if (Input.GetKeyDown(KeyCode.I))
    {
   		WhereIsAvatar="indoor";
    	Stockmanns=GameObject.FindGameObjectsWithTag("Stockmann");
		for (Stockmann in Stockmanns) 
		{
			Stockmann.GetComponent.<Renderer>().enabled=false;
		}
		Indoors=GameObject.FindGameObjectsWithTag("Indoor");
		for (Indoor in Indoors) 
		{
			Indoor.GetComponent.<Renderer>().enabled=true;
		}
		Outdoors=GameObject.FindGameObjectsWithTag("Outdoor");
		for (Outdoor in Outdoors) 
		{
			Outdoor.GetComponent.<Renderer>().enabled=false;
		}
		Target = GameObject.Find("SMDImport_Stockmann");
		Target.GetComponent.<Renderer>().enabled=false;
		Target = GameObject.Find("SMDImport_Indoor");
		Target.GetComponent.<Renderer>().enabled=true;
		
		var Eye : GameObject = GameObject.Find("Eye");
	    var EyeSocket : GameObject = GameObject.Find("Eyesocket");
	    

	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Marjatta_Indoor");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 80 ,Target.transform.position.z-120) ;

    }
    
    if (Input.GetKeyDown(KeyCode.O))
    {
		WhereIsAvatar="outdoor";

		Target = GameObject.Find("SMDImport_Stockmann");
		Target.GetComponent.<Renderer>().enabled=false;
		Target = GameObject.Find("SMDImport_Indoor");
		Target.GetComponent.<Renderer>().enabled=false;
		
		Eye = GameObject.Find("Eye");
		EyeSocket = GameObject.Find("Eyesocket");
	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Z_OldBird1_Red_1");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 1080 ,Target.transform.position.z-120) ;


    }
    if (Input.GetKeyDown(KeyCode.P))
    {
   		WhereIsAvatar="Stockmann";
    	Stockmanns=GameObject.FindGameObjectsWithTag("Stockmann");
		for (Stockmann in Stockmanns) 
		{Stockmann.GetComponent.<Renderer>().enabled=true;}
		Indoors=GameObject.FindGameObjectsWithTag("Indoor");
		for (Indoor in Indoors) 
		{Indoor.GetComponent.<Renderer>().enabled=false;}
		Outdoors=GameObject.FindGameObjectsWithTag("Outdoor");
		for (Outdoor in Outdoors) 
		{Outdoor.GetComponent.<Renderer>().enabled=false;}
		Target = GameObject.Find("SMDImport_Indoor");
		Target.GetComponent.<Renderer>().enabled=false;
		Target = GameObject.Find("SMDImport_Stockmann");
		Target.GetComponent.<Renderer>().enabled=true;
		
		Eye = GameObject.Find("Eye");
		EyeSocket = GameObject.Find("Eyesocket");
	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Marjatta_Stockmann");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 80 ,Target.transform.position.z-120) ;

    }
    
    if (Input.GetKeyDown(KeyCode.Z))
    {
    	Eye = GameObject.Find("Eye");
	    EyeSocket = GameObject.Find("Eyesocket");
    	if(WhereIsAvatar=="indoor")
	    {
	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Marjatta_Indoor");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 80 ,Target.transform.position.z-120) ;
	    }
	    if(WhereIsAvatar=="outdoor")
	    {
	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Z_OldBird1_Red_1");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 1080 ,Target.transform.position.z-120) ;
	    }
	    if(WhereIsAvatar=="Stockmann")
	    {
	    Eye.GetComponent.<Camera>().orthographicSize = 8;
	    Target = GameObject.Find("Marjatta_Stockmann");
	    EyeSocket.transform.position=Vector3(Target.transform.position.x , 80 ,Target.transform.position.z-120) ;
	    }
	}
}

function Mycoroutine(): IEnumerator {
	
	executingCorouti=true;
	
		if(WhereIsAvatar=="Stockmann")
	{
		sx4=sx2;
		sz4=sz2;			
		if(StockmannStartUpdate==false)
		{
			StockmannStartUpdate=true;
			
			Stockmanns=GameObject.FindGameObjectsWithTag("Stockmann");
			for (Stockmann in Stockmanns) 
			{Stockmann.GetComponent.<Renderer>().enabled=true;}
			Indoors=GameObject.FindGameObjectsWithTag("Indoor");
			for (Indoor in Indoors) 
			{Indoor.GetComponent.<Renderer>().enabled=false;}
			Outdoors=GameObject.FindGameObjectsWithTag("Outdoor");
			for (Outdoor in Outdoors) 
			{Outdoor.GetComponent.<Renderer>().enabled=false;}
			
			if(steta==0)
			{
			srotationy= 90;
			transform.Rotate(0, srotationy, 0);
			steta=1;
			}
			
			var StockmannurliA = "https://dl.dropbox.com/u/106820791/OldBird_WebsiteHost/StockmannA.txt";
			var StockmannwwwiA : WWW = new WWW(StockmannurliA);
			yield StockmannwwwiA;
			stxtA=StockmannwwwiA.text;
			
			var StockmannurliB = "https://dl.dropbox.com/u/106820791/OldBird_WebsiteHost/StockmannB.txt";
			var StockmannwwwiB : WWW = new WWW(StockmannurliB);
			yield StockmannwwwiB;
			stxtB=StockmannwwwiB.text;
			
			var Stockmannsr1 = stxtA;
			skama=Stockmannsr1.IndexOf(",");
			stempA=Stockmannsr1.Substring(0,skama);
			sx1=parseFloat(stempA);
			stempA=Stockmannsr1.Substring(skama+1);
			sz1=parseFloat(stempA);
			
			var Stockmannsr2 = stxtB;
			skama=Stockmannsr2.IndexOf(",");
			stempB=Stockmannsr2.Substring(0,skama);
			sx2=parseFloat(stempB);
			stempB=Stockmannsr2.Substring(skama+1);
			sz2=parseFloat(stempB);
		
			
			sx=sx1;
			sz=sz1;
			transform.position=Vector3(sx1,4,sz1);
			
			var StockmannEye : GameObject = GameObject.Find("Eye");
			var StockmannEyeSocket : GameObject = GameObject.Find("Eyesocket");
			StockmannEye.GetComponent.<Camera>().orthographicSize = 8;
		    StockmannEyeSocket.transform.position=Vector3(sx1 , 80 ,sz1-120) ;
		    
		}
			
			
			StockmannurliB = "https://dl.dropbox.com/u/106820791/OldBird_WebsiteHost/StockmannB.txt";
			StockmannwwwiB = WWW(StockmannurliB);
			yield StockmannwwwiB;
			stxtB=StockmannwwwiB.text;
			
			
			Stockmannsr2 = stxtB;
			skama=Stockmannsr2.IndexOf(",");
			stempB=Stockmannsr2.Substring(0,skama);
			sx2=parseFloat(stempB);
			stempB=Stockmannsr2.Substring(skama+1);
			sz2=parseFloat(stempB);
			
			
			if(sx2 != sx4 || sz2 != sz4){
			scharkheshA=1;
			sharekatA=0;
			sentezarA=0;
			}
	}	
	
	executingCorouti=false;
	}
	
function scharkhesh (){
		if(WhereIsAvatar=="Stockmann")
	{
		//animation.Play("Take 001");
		sx3=sx2-sx1;
		sz3=sz2-sz1;
		steta1=((Mathf.Atan(sz3/sx3))*180)/Mathf.PI;
		if(sx2>sx1 && sz2>sz1){}
		if(sx2<sx1 && sz2>sz1){steta1=steta1+180;}
		if(sx2<sx1 && sz2<sz1){steta1=steta1+180;}
		if(sx2>sx1 && sz2<sz1){steta1=steta1+360;}
		
		if(steta1<=180){
		transform.Rotate(0,-0.5,0);
		snewteta=snewteta+0.5;
		}
		if(steta1>180){
		transform.Rotate(0,0.5,0);
		snewteta=snewteta-0.5;
		}
		if (snewteta == 360) {snewteta=0;}
		if (snewteta < 0) {snewteta=snewteta+360;}
		if ((steta1-snewteta) < 1 && (steta1-snewteta)>-1 )
		{
		scharkheshA=0;
		sharekatA=1;
		}
	}
}
	
function sharekat (){
	if(WhereIsAvatar=="Stockmann")
	{	
		//animation["Take 002"].speed=0.2;
		//animation.Play("Take 002");
		sx1=sx1+((Mathf.Cos(snewteta*Mathf.PI/180))/60);
		sz1=sz1+((Mathf.Sin(snewteta*Mathf.PI/180))/60);
		transform.position=Vector3(sx1,4,sz1);
		if( (sz2-sz1) <1 && (sx2-sx1) <1 && (sz2-sz1)>-1 && (sx2-sx1)>-1)
		{
		sharekatA=0;
		sentezarA=1;
		}		
	}
}

function sentezar (){
	if(WhereIsAvatar=="Stockmann")
	{	
		transform.position=Vector3(sx1,4,sz1);
		//animation.Play("Take 001");
	}	
}
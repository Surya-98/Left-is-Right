using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bose.Wearable;

public class main : MonoBehaviour
{
	public AudioClip left;
	public AudioClip right;
	public AudioClip back;
	public AudioClip front;
	public AudioClip GS;
	public AudioClip GO;
	public AudioClip FB_flip;
	public AudioClip LR_flip;
	public AudioClip B_flip;
	public AudioClip Next_Level;
	public AudioClip Won;			//10
	public AudioClip Life_Lost;
	public AudioClip Extra_Life;
	public AudioClip Menu;
	public AudioClip Help;
	public AudioClip Correct;  
	public AudioSource leftspeaker;
	public GameObject Instruction;
	public int GameMode = 0;
	public double movex = 0;
	public double movez = 0;
	public double timeold = 0;
	public double reaction_time = 3.0;
	public double delay = 3.0;
	public int r,r1;
	public int r_o;
	public int initialize = 0;
	public int lr_flip = 0;
	public int fb_flip = 0;
	public int ctr = 0;
	public int difficulty = 0;
	public int info_mode = 0;
	public int info_mode1 = 0;
	public int threshold = 5;
	public int Lives = 3;
	public int Cont_Correct = 0;
	public int score = 0;
	public WearableControl wearableControl;
	public Text T_Centr_L1 = null;
	public Text T_Score = null;
	public Text T_Lives = null;
	public Text T_Difficulty = null;
	public Text T_LRStatus = null;
	public Text T_FBStatus = null;
	public Text T_Hint = null;
    // Start is called before the first frame update
    void Start()
    {
		T_Centr_L1.text = "Welcome to the Game";
		T_Score.text = " ";
		T_Lives.text = " ";
		T_Difficulty.text = " ";
		T_LRStatus.text = " ";
		T_FBStatus.text = " ";
		T_Hint.text = " ";
		Instruction.SetActive(false);
    }
	
	void reset_variables(){
		GameMode = 0;
		ctr = 0;
		info_mode = 0;
		info_mode1 = 0;
		lr_flip = 0;
		fb_flip = 0;
		difficulty = 0;
		reaction_time = 3;
		Lives = 3;
		Cont_Correct = 0;
		timeold = Time.time;
		score = 0;
	}
	
	void playaudio(int audio_num)
	{
		if (audio_num == 0){
			leftspeaker.clip = front;
			T_Centr_L1.text = "Front";
		}
		else if (audio_num == 1){
			leftspeaker.clip = right;
			T_Centr_L1.text = "Right";
		}
		else if (audio_num == 2){
			leftspeaker.clip = back;
			T_Centr_L1.text = "Back";
		}
		else if (audio_num == 3){
			leftspeaker.clip = left;
			T_Centr_L1.text = "Left";
		}
		else if (audio_num == 4){		//Game Start
			leftspeaker.clip = GS;
			T_Centr_L1.text = "Game Started";
			T_Score.text = "Score : " + score;
			T_Lives.text = "Lives : " + Lives;
			T_Difficulty.text = "Difficulty : " + difficulty;
			T_FBStatus.text = " ";
			T_LRStatus.text = " ";
			T_Hint.text = "Press 'h' for hint";
		}
		else if (audio_num == 5){		//Game Over
			leftspeaker.clip = GO;
			T_LRStatus.text = " ";
			T_FBStatus.text = " ";
			T_Hint.text = "";
			reset_variables();
		}
		else if (audio_num == 6){		//Left Right Flip
			leftspeaker.clip = LR_flip;
			T_Centr_L1.text = "Next Level : Left & Right Flipped";
			if (lr_flip==0)
				lr_flip = 1;
			else
				lr_flip = 0;
		}
		else if (audio_num == 7){		//Front Back Flip
			leftspeaker.clip = FB_flip;
			T_Centr_L1.text = "Next Level : Front & Back Flipped";
			if (fb_flip==0)
				fb_flip = 1;
			else
				fb_flip = 0;
		}
		else if (audio_num == 8){		//Both Flip
			leftspeaker.clip = B_flip;
			T_Centr_L1.text = "Next Level : Both Flipped";
			if (fb_flip==0)
				fb_flip = 1;
			else
				fb_flip = 0;
			if (lr_flip==0)
				lr_flip = 1;
			else
				lr_flip = 0;
		}
		else if (audio_num == 9)		//Next Level
			leftspeaker.clip = Next_Level;
		else if (audio_num == 10){		//Won
			leftspeaker.clip = Won;
			T_Centr_L1.text = "Congratulations\nYou have WON!!!";
			reset_variables();
		}
		else if (audio_num == 11){		//Life Lost
			leftspeaker.clip = Life_Lost;
			T_Centr_L1.text = "Incorrect\nLife Lost";
		}
		else if (audio_num == 12){		//Extra Life
			leftspeaker.clip = Extra_Life;
			T_Centr_L1.text = "Level Successful\nExtra Life Received";
		}
		else if (audio_num == 13){		//Help
			leftspeaker.clip = Help;
			T_Centr_L1.text = " ";
		}
		else if (audio_num == 14){		//Menu
			leftspeaker.clip = Menu;
			T_Centr_L1.text = "Menu\n1 : Instructions\n2 : Start Game";
		}
		else if (audio_num == 15){		//Correct
			leftspeaker.clip = Correct;
			T_Centr_L1.text = "Correct";
		}
		
		if (audio_num <= 3)
			delay = reaction_time + leftspeaker.clip.length;
		else if (audio_num == 14)
			delay = 10;
		else
			delay = 0.25 + leftspeaker.clip.length;
		leftspeaker.Play(0);
		//Debug.Log(leftspeaker.clip);
	}
	
	void Wrong_Action(){
		Lives--;
		T_Lives.text = "Lives : " + Lives;
		if (Lives==0){
			playaudio(5);
		}
		else{
			playaudio(11);
			Cont_Correct = 0;
			info_mode1 = 1;
		}
	}
    // Update is called once per frame
	// Mode - 1:Help, 2:Classic, 3:Memory
    void Update()
    {	
		if(Input.GetKeyDown("1")){
			if (GameMode==0){
				GameMode = 1;
				Instruction.SetActive(true);
				//playaudio(13);
				timeold = Time.time;
			}
			else if (GameMode == 1){
				GameMode = 0;
				Instruction.SetActive(false);
				timeold = Time.time;
			}
		}
		else if(Input.GetKeyDown("2")){
			if (initialize==0){
				wearableControl = WearableControl.Instance;
				WearableRequirement requirement = GetComponent<WearableRequirement>();
		    	if (requirement == null)
		    	    requirement = gameObject.AddComponent<WearableRequirement>();
		    	
		    	requirement.EnableSensor(SensorId.RotationSixDof);
		    	requirement.SetSensorUpdateInterval(SensorUpdateInterval.FortyMs);
				//Debug.Log("Game Started : i = "+ i);
				initialize = 1;
			}
			GameMode = 2;
			playaudio(4);
			
			timeold = Time.time;
		}
		else if(Input.GetKeyDown("h")){
			score = score - 100;
			T_Score.text = "Score : " + score;
			if (fb_flip==1)
				T_FBStatus.text = "Front-Back : Flipped";
			else
				T_FBStatus.text = "Front-Back : Normal";
			if (lr_flip==1)
				T_LRStatus.text = "Left-Right : Flipped";
			else
				T_LRStatus.text = "Left-Right : Normal";
			T_Hint.text = "";
		}
		
		if (GameMode > 1){
			SensorFrame sensorFrame = wearableControl.LastSensorFrame;				
			movez = sensorFrame.rotationSixDof.value.eulerAngles.z;
        	movex = sensorFrame.rotationSixDof.value.eulerAngles.x;
		}
		
		if (GameMode == 0){
			if(Time.time-timeold >= delay){
				playaudio(14);
				timeold = Time.time;
			}
		}
		/*else if (GameMode == 1){
			if(Time.time-timeold >= delay){
				GameMode = 0;
				T_Score.text = " ";
				T_Lives.text = " ";
				T_Difficulty.text = " ";
				T_LRStatus.text = " ";
				T_FBStatus.text = " ";	
			}
		}*/
		else if (GameMode == 2){
	        if(Time.time-timeold >= delay){
				//Debug.Log("Actually Started : i = "+ i);
				info_mode1 = 0;
				// Checking the head position : Pitch & Roll	
				if(ctr!=0 && info_mode==0)
				{	//Debug.Log(r_o+","+movex+","+movez);
					//Debug.Log("Before Checking : i = "+ i);
					if (lr_flip == 0){
						if(r_o == 1 && !((movez>270)&&(movez<360-threshold)))
							Wrong_Action();
						else if(r_o == 3 && !((movez>threshold)&&(movez<90)))
							Wrong_Action();
					}
					else{
						if(r_o == 3 && !((movez>270)&&(movez<360-threshold)))
							Wrong_Action();
						else if(r_o == 1 && !((movez>threshold)&&(movez<90)))
							Wrong_Action();		
					}
					if (fb_flip == 0){
						if(r_o == 0 && !((movex>threshold)&&(movex<90)))
							Wrong_Action();
						else if(r_o == 2 && !((movex>270)&&(movex<360-threshold)))
							Wrong_Action();
					}
					else{
						if(r_o == 2 && !((movex>threshold)&&(movex<90)))
							Wrong_Action();
						else if(r_o == 0 && !((movex>270)&&(movex<360-threshold)))
							Wrong_Action();
					}
					
				}
				
				if (ctr>0 && info_mode == 0 && info_mode1 == 0){
					score = score + difficulty;
					T_Score.text = "Score : " + score;
					Cont_Correct++;
					playaudio(15);
					info_mode1 = 1;
			
					if (Cont_Correct == 5){
						Cont_Correct = 0;
						Lives++;
						playaudio(12);
						T_Lives.text = "Lives : " + Lives;
						//info_mode1=1;
					}
				}
				info_mode = info_mode1;
				
				// Change level after every 5 steps
				if (GameMode != 0 && ctr == 5 && info_mode == 0){
					difficulty++;
					T_FBStatus.text = " ";
					T_LRStatus.text = " ";
					T_Hint.text = "Press 'h' for hint";
					Cont_Correct = 0;
					reaction_time = reaction_time*(0.9);
					if (difficulty == 16){
						playaudio(10);
					}
					else{	
						r1 = Random.Range(1,4);		// 1:LR_Flip, 2:FB_Flip, 3:Both
						if (difficulty <= 5){
							while (r1==3)
								r1 = Random.Range(1,4);
						}
						playaudio(5+r1);			
						ctr = 0;
					}
					info_mode = 1;
					T_Difficulty.text = "Difficulty : " + difficulty;
				}
					 
				timeold = Time.time;
				if(GameMode != 0 && info_mode == 0){	
					//Debug.Log("Before Playing : i = "+ i);
					r = Random.Range(0,4);
					while (r==r_o){
						r = Random.Range(0,4);
					}
					r_o = r;
					playaudio(r);
					ctr++;
				}
				
			}
		}
	}
}

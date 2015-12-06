using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScene : MonoBehaviour {

	public float GameTime = 60f;
	private float currentTime = .0f;
	private float correctAnswer = .0f;
	private bool isStart = false;
	private PhotonView myPv;

	Text playerCountText;
	Text timeText;
	Text[] buttonText;
	Text scoreText;
	Text questionText;
	private int difficulty = 1;
	// Use this for initialization
	void Start () {
		buttonText = new Text[4];
		timeText = GameObject.Find ("TimeText").GetComponent<Text> ();
		for (int i = 0; i < 4; ++i) {
			int buttonNo = i + 1;
			var goParent = GameObject.Find ("AnswerButton" + buttonNo);
			var go = goParent.transform.FindChild ("Text").gameObject;
			buttonText[i] = go.GetComponent<Text> ();
		}
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		questionText = GameObject.Find ("QuestionText").GetComponent<Text> ();
		playerCountText = GameObject.Find ("PlayerCountText").GetComponent<Text> ();

		myPv = this.GetComponent<PhotonView>();
		GameManager.instance.Score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart) {
			playerCountText.text = "PlayerCount:" + PhotonNetwork.playerList.Length;	
			return;
		}

		// time update
		currentTime += Time.deltaTime;
		float leftTime = GameTime - currentTime;
		if (leftTime <= 0) {
			leftTime = .0f;
			timeText.text = ((int)leftTime).ToString ();
			PhotonNetwork.Disconnect ();
			Application.LoadLevel ("Result");
		}
		timeText.text = ((int)leftTime).ToString ();
	}

	[PunRPC]
	void addScore(int score){
		GameManager.instance.Score = score;
		scoreText.text = GameManager.instance.Score.ToString ();
	}

	[PunRPC]
	void start(){
		isStart = true;
		updateQuesttion ();
	}

	private void updateQuesttion() {
		Debug.LogFormat ("Q_level:{0}, difficulty:{1}", GameManager.instance.SelectLevel, difficulty);

		switch (GameManager.instance.SelectLevel) {
		case 1:
			calcQuestionDigit2 ();
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		default:
			Debug.LogWarning("wrong at switch");
			break;
		}


		int answerNo = Random.Range (0, 4);
		for (int i = 0; i < buttonText.Length; ++i) {
			buttonText [i].text = (correctAnswer - (answerNo - i)).ToString ();
		}
	}

	private void calcQuestionDigit2() {
		int type = -1;
		int arg1Digit10 = -1;
		int arg1Digit1 = -1;
		int arg2Digit10 = -1;
		int arg2Digit1 = -1;

		int arg1 = -1;
		int arg2 = -1;


		switch (difficulty) {
		case 1:
			type = Random (1, 5);
			Debug.LogFormat ("calcQuestionDigit2: q_type:{0}", type);

			switch(type){
			case 1:		//oo+o
				arg1Digit10 = Random.Range(1, 10);
				arg1Digit1 = Random.Range(1, 9);
				arg2Digit10 = 0;
				arg2Digit1 = Random.Range(1, 10-arg1Digit1);
				
				arg1 = arg1Digit1 + arg1Digit10;
				arg2 = arg2Digit10 + arg2Digit1;
				correctAnswer = arg1 + arg2;
				questionText.text = arg1.ToString () + " + " + arg2.ToString() + " = ?";
				break;
			case 2:
				arg1Digit10 = Random.Range(1, 10);
				arg1Digit1 = Random.Range(1, 9);
				arg2Digit10 = Random.Range(1, 10-arg1Digit10);
				arg2Digit1 = Random.Range(1, 10-arg1Digit1);
				
				arg1 = arg1Digit1 + arg1Digit10;
				arg2 = arg2Digit10 + arg2Digit1;
				correctAnswer = arg1 + arg2;
				questionText.text = arg1.ToString () + " + " + arg2.ToString() + " = ?";
				break;
			default:
				Debug.LogWarning("wrong at switch");
				break;
			}
			break;

		case 2:
			type = Random (1, 5);
			Debug.LogFormat ("calcQuestionDigit2 q_type:{0}", type);
			switch(type){
			case 1:
				arg1Digit10 = Random.Range(1, 10);
				arg1Digit1 = Random.Range(0, 10);
				arg2Digit10 = 0;
				arg2Digit1 = Random.Range(0, 10);
				
				arg1 = arg1Digit1 + arg1Digit10;
				arg2 = arg2Digit10 + arg2Digit1;
				correctAnswer = arg1 + arg2;
				questionText.text = arg1.ToString () + " + " + arg2.ToString() + " = ?";
				break;
			case 2:				
				arg1Digit10 = Random.Range(0, 10);
				arg1Digit1 = Random.Range(0, 10);
				arg2Digit10 = Random.Range(0, 10);
				arg2Digit1 = ((arg2Digit10!=0) ! Random.Range(0, 10): Random.Range(1, 10));
				
				arg1 = arg1Digit1 + arg1Digit10;
				arg2 = arg2Digit10 + arg2Digit1;
				correctAnswer = arg1 + arg2;
				questionText.text = arg1.ToString () + " + " + arg2.ToString() + " = ?";
				break;
			default:
				Debug.LogWarning("wrong at switch");
				break;
			}
			break;

		case 3:
			type = Random (1, 5);
			Debug.LogFormat ("calcQuestionDigit2 q_type:{0}", type);
			switch(type){
			case 1:
				arg1Digit10 = Random.Range(1, 10);
				arg1Digit1 = Random.Range(0, 10);
				arg2Digit10 = 100;
				arg2Digit1 = 100;
				
				arg1 = arg1Digit1 + arg1Digit10;
				arg2 = arg1+Random.Range(1, 10);
				correctAnswer = arg2 - arg1;
				questionText.text = arg1.ToString () + " + ? = " + arg2.ToString();
				break;
			default:
				Debug.LogWarning("wrong at switch");
				break;
			}
			break;
		default:
			Debug.LogWarning("wrong at switch");
			break;
		}

		if (arg1Digit10 < 0) 	Debug.LogWarningFormat("arg1Digit10:{0}"	, arg1Digit10);
		if (arg1Digit1 < 0) 	Debug.LogWarningFormat("arg1Digit1:{0}"	, arg1Digit1);
		if (arg2Digit10 < 0) 	Debug.LogWarningFormat("arg2Digit10:{0}"	, arg2Digit10);
		if (arg2Digit1 < 0) 	Debug.LogWarningFormat("arg2Digit1:{0}"	, arg2Digit1);
		if (arg2Digit10 < 0) 	Debug.LogWarningFormat("arg2Digit10:{0}"	, arg2Digit10);
		if (arg2Digit1 < 0) 	Debug.LogWarningFormat("arg2Digit1:{0}"	, arg2Digit1);
	}

	private void calcQuestionDigit3() {
		int digit = (int)Mathf.Pow (10, difficulty);
		int arg1 = 0;
		int arg2 = 1;
		correctAnswer = arg1 + arg2;
		questionText.text = arg1.ToString () + " + " + arg2 + " = ?";
	}

	public void onAnswerClick(int buttonNo) {
		if (!isStart) {
			return;
		}

		float answer = float.Parse (buttonText [buttonNo].text);
		if (correctAnswer == answer) {
			GameManager.instance.Score += 30;
			scoreText.text = GameManager.instance.Score.ToString ();
			myPv.RPC ("addScore", PhotonTargets.All, GameManager.instance.Score);
			difficulty++;
			if (difficulty > 3) {
				difficulty = 3;
			}
		} else {
			difficulty--;
			if (difficulty < 1) {
				difficulty = 1;
			}
		}
		updateQuesttion ();
	}

	public void onStart() {
		isStart = true;
		myPv.RPC("start",PhotonTargets.All);
	}
}

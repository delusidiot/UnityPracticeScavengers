﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	public BoardManager boardScript;
	private int level = 3;
	
	void Awake(){
		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

	void InitGame(){
		boardScript.SetupScenes(level);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

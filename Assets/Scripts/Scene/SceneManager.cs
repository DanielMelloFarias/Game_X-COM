﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
        //SceneManager.LoadScene;
    }

	public void ExitGame()
	{
		Application.Quit();
	}
}

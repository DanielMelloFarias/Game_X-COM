using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    /*
        TurnManager :: endTurn();
            1. currentPlayer.Lock();
            2. set currentPlayer to next Player (Players[index++ % = 2])
            3. currentPlayers.startTurn()   
    */

public class TurnManager : MonoBehaviour
{
    int turnCounter = 1; //Initializes turn counter
    Player [] player; //Creates array for the two store two Player class objects
    Player currentPlayer;  //Variable to represent the current player
   // currentPlayer = player[0]; //Assigns the current Player to the first Player in player[]
    int index = 0;
	public int maxTurn;
	public GameObject winUI;
	public GameObject GridUI;
	public GameObject unitsGO;
	public Text winText;
	
	public AudioScript audioScript;
    
    public static TurnManager turnManager;
	int nextIndex
	{
		get
		{
			return (index + 1) % 2;
		}
	}
	private void Awake()
	{
		player = new Player[2];
		initSingleton();
	}
	private void Start()
	{
		player[0] = PlayerManager.playerManager.players[0];
		player[1] = PlayerManager.playerManager.players[1];
		currentPlayer = player[0]; //Assigns the current Player to the first Player in player[]
        player[1].Lock();
		player[1].disableUnitBuilding();
		UIManager.uiManager.UpdateUI(currentPlayer, turnCounter);
	}
	/*
        EndTurn() allows the current player to end t
        heir turn, locking them in from further input during
        the next Player's turn, assigns the currentPlayer to the next Player in player[], and increments the turn counter by 1.
    */

	public void EndTurn()
    {
        UnitManager.unitManager.selectedUnit = null;
        currentPlayer.Lock(); //Locks the current Player object
        index++;
		index %= 2;
		currentPlayer.disableUnitBuilding();
        currentPlayer = player[index]; //Assigns currentPlayer to the next Player object in player[]
		turnCounter++; //Increments turn counter by 1
		PlayerManager.playerManager.SetActivePlayer(currentPlayer); // update player manager's active players
		if (turnCounter >= maxTurn)
		{
			DetermineWinner();
			return;
		}
		currentPlayer.startTurn(); //Calls on the Player's startTurn function

		UIManager.uiManager.UpdateUI(currentPlayer, turnCounter);
        audioScript.SwitchMusic(turnCounter);

		//Debug.Log(unitsGO.transform.childCount);
		// Passa por todos os obj e poe o valor de movimentos
		for (int i = 0; i < unitsGO.transform.childCount; i++)
		{
			Transform children = unitsGO.transform.GetChild(i);
			//Debug.Log(children.name);
			children.gameObject.GetComponentInChildren<Text>().text = children.GetComponent<Unit>().getmovement().ToString();
		}


	}

    void initSingleton()
    {
        if(turnManager == null)
            turnManager = this;
        else if (turnManager != this)
            Destroy(gameObject);
    }

	void DetermineWinner()
	{
		if (player[0].GetWinSum() > player[1].GetWinSum())
			Win(player[0]);
		else if (player[1].GetWinSum() > player[0].GetWinSum())
			Win(player[1]);
		else
			Win(null);
	}

	void Win(Player winner)
	{
		/*
		 display win UI
		 if (winner == null) ui text = "DRAW"
		 else
			 UI text = winner.name

		if user clicks exit button, return to main menu
		 */
		if (winner == null)
			winText.text = "Draw";
		else
			winText.text = winner.name + " Wins!";

		GridUI.SetActive(false);
		winUI.SetActive(true);
	}

	public void Lose(Player loser)
	{
		// called on by Player class. Loser player is passed in as parameter
		if (loser.name == "Player1")
			winText.text = "Player2 Wins!";
		else
			winText.text = "Player1 Wins!";

		GridUI.SetActive(false);
		winUI.SetActive(true);
	}

    void start()
    {
        
    }   

    void update()
    {
		if (player[nextIndex].units.Count == 0 && player[nextIndex].buildings.Count == 0)
			Win(player[index]);
		else if (player[index].units.Count == 0 && player[index].buildings.Count == 0)
			Win(player[nextIndex]);
    }
}
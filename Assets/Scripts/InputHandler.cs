using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InputHandler : MonoBehaviour
{

	//The different keys we need
	private Command w, a, s, d;
	//Stores all commands for replay and undo
	public static List<Command> oldCommands = new List<Command>();

	GameObject Player;
	PlayerController pc;

	void Start()
	{
		//Bind keys with commands
		w = new MoveUp();
		a = new MoveLeft();
		s = new MoveDown();
		d = new MoveRight();
		Player = GameObject.FindWithTag("Player");
		pc = Player.GetComponent<PlayerController>();
	}



	void Update()
	{
			HandleInput();
	}


	//Check if we press a key, if so do what the key is binded to 
	public void HandleInput()
	{
		if (Input.GetKey(KeyCode.W) && pc.Grounded)
		{
			w.Execute(Player);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			a.Execute(Player);
		}
		else if (Input.GetKey(KeyCode.S) && pc.Grounded)
		{
			s.Execute(Player);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			d.Execute(Player);
		}
	}
}


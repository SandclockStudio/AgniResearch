using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InputHandler : MonoBehaviour
{
	//The box we control with keys
	public Transform Player;
	//The different keys we need
	private Command w, a, s, d;
	//Stores all commands for replay and undo
	public static List<Command> oldCommands = new List<Command>();

	GameObject playerObject;
	PlayerBehavior pb;

	void Start()
	{
		//Bind keys with commands
		w = new MoveUp();
		a = new MoveLeft();
		s = new MoveDown();
		d = new MoveRight();
		playerObject = GameObject.FindWithTag("Player");
		pb = playerObject.GetComponent<PlayerBehavior>();
	}



	void Update()
	{
			HandleInput();
	}


	//Check if we press a key, if so do what the key is binded to 
	public void HandleInput()
	{
		if (Input.GetKey(KeyCode.W) && pb.notGrounded())
		{
			w.Execute(Player, w);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			a.Execute(Player, a);
		}
		else if (Input.GetKey(KeyCode.S) && pb.notGrounded())
		{
			s.Execute(Player, s);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			d.Execute(Player, d);
		}
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//The parent class
public abstract class Command
{
	//Move 
	public abstract void Execute(GameObject Player);

}


//
// Child classes
//

public class MoveUp : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.up);
	}
}


public class MoveDown : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.down);
	}
}


public class MoveLeft : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.left);
	}
}


public class MoveRight : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.right);
	}
}
	


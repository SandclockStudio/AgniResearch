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
		mb.SetDirection(Vector3.up,1);
	}
}


public class MoveDown : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.down,1);
	}
}


public class MoveLeft : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.left,1);
	}
}


public class MoveRight : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.SetDirection(Vector3.right,1);
	}
}


public class Jump : Command
{
	//Called when we press a key
	public override void Execute(GameObject Player)
	{
		//Move the box
		MovementBehaviour mb = Player.GetComponent<MovementBehaviour>();
		mb.Jump();
	}
}
	


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//The parent class
public abstract class Command
{
	//How far should the box move when we press a button
	protected float Speed = 0.1f;

	//Move 
	public abstract void Execute(Transform Player, Command command);

	//Move the box
	public virtual void Move(Transform Player) { }
}


//
// Child classes
//

public class MoveUp : Command
{
	//Called when we press a key
	public override void Execute(Transform Player, Command command)
	{
		//Move the box
		Move(Player);
	}

	//Move the box
	public override void Move(Transform Player)
	{
		Player.Translate(Player.up * Speed);
	}
		
}


public class MoveDown : Command
{
	//Called when we press a key
	public override void Execute(Transform Player, Command command)
	{
		//Move the box
		Move(Player);
	}

	//Move the box
	public override void Move(Transform Player)
	{
		Player.Translate(-Player.up * Speed);
	}
}


public class MoveLeft : Command
{
	//Called when we press a key
	public override void Execute(Transform Player, Command command)
	{
		//Move the box
		Move(Player);
	}
		
	//Move the box
	public override void Move(Transform Player)
	{
		Player.Translate(-Player.right * Speed);
	}
}


public class MoveRight : Command
{
	//Called when we press a key
	public override void Execute(Transform Player, Command command)
	{
		//Move the box
		Move(Player);

	}

	//Move the box
	public override void Move(Transform Player)
	{
		Player.Translate(Player.right * Speed);
	}
}
	


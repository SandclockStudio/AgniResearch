using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriangle 
{
	private int[] triangles;
	float targetTime;
	public bool delete;

	public MyTriangle(int [] triangles, float targetTime)
	{
		this.triangles = triangles;
		this.targetTime = targetTime;
		delete = false;
	}

	public void update()
	{
		targetTime -= Time.deltaTime;
	}

	public float getTime()
	{
		return targetTime;
	}

	public int[] getTriangles()
	{
		return triangles;
	}

}

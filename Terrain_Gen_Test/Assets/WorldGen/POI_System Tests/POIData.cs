using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[System.Serializable]
public class POIData
{
	public Vector2 xPos;	
	public int baseSize;
	public float areaSize;
	[SerializeField]	
	POI_DataObject POI_Template;
	int seed;

	public POIData()
	{

	}




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New POIDataModifier")]
public class POI_NoiseModifier : TerrainNoiseModifier
{
	[SerializeField]
	List<POIData> POI_data;
	[SerializeField]
	float height;

	public override float GetRatio(float pointX, float pointY)
	{
		//This needs to return the largest Ratio(smallist distance) of all the POIdata
		POIData focus = null;
		float minDis = 200f;
		foreach(POIData poi in POI_data)
		{
			if(minDis> Vector2.Distance(poi.xPos, new Vector2(pointX, pointY))){
				minDis = Mathf.Min(Vector2.Distance(poi.xPos, new Vector2(pointX, pointY)), minDis);
				focus = poi;
			}
			

		}
		return 1-(minDis / (focus != null ? focus.areaSize:1));
	}

	public override float GetTargetHeight(float pointX, float pointY)
	{
		return height;
	}
}

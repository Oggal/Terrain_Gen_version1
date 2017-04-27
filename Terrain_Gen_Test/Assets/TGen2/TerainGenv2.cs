using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerainGenv2
{

	private TerrainController.ChunkSettings settings { get; set; }
	private TerrainController TerrainCtrl;
	private Terrain _Terrain;

	public int X { get; set; }
	public int Z { get; set; }

	public TerainGenv2( TerrainController TC,int x,int z)
	{
		TerrainCtrl = TC;
		X = x;
		Z = z;
		settings = new TerrainController.ChunkSettings(1025, 129, 1000, 400);
	}

	public void CreateTerrain()
	{
		var TerrainDat = new TerrainData();
		TerrainDat.heightmapResolution = settings.HeightmapResolution;
		TerrainDat.alphamapResolution = settings.AplhamapResolution;

		var heightmap = GetHeightMap();
		TerrainDat.SetHeights(0, 0, heightmap);
		TerrainDat.size = new Vector3(settings.Length, settings.Height, settings.Length);

		var TerrainGameObj = Terrain.CreateTerrainGameObject(TerrainDat);
		TerrainGameObj.transform.position = new Vector3(X * settings.Length, 0, Z * settings.Length);
		_Terrain = TerrainGameObj.GetComponent<Terrain>();
        if(GlobalDataController.GameControler != null)
            _Terrain.gameObject.transform.parent = GlobalDataController.GameControler.WorldObj.transform;
		_Terrain.Flush();
	}

	private float[,] GetHeightMap()
	{
		var heightmap = new float[settings.HeightmapResolution, settings.HeightmapResolution];
		for (int zRes = 0; zRes < settings.HeightmapResolution; zRes++)
		{
			for (int xRes = 0; xRes < settings.HeightmapResolution; xRes++)
			{
				float xCoord = (X + (float)xRes / (settings.HeightmapResolution - 1))*settings.Length;
				float zCoord = (Z + (float)zRes/ (settings.HeightmapResolution - 1)) * settings.Length;
				heightmap[zRes, xRes] = (TerrainCtrl.getHeight(xCoord, zCoord))/10f+0.5f;
			}
		}
		return heightmap;
	}

}



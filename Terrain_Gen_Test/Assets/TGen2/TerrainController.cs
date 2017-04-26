using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
public class TerrainController
{

	private byte octaves;
	public TerrainNoise Noise;

	public TerrainController()
	{
		octaves = 4;
	}

	public void Test()
	{
		Noise = new TerrainNoise("TestSeed",2);
		for(int x = -1; x < 2; x++)
		{
			for(int y = -1; y < 2; y++)
			{
				TerainGenv2 TG = new TerainGenv2(this, x, y);
				TG.CreateTerrain();
			}
		}
	
	}

	public class ChunkSettings
	{
		public int HeightmapResolution { get; private set; }
		public int AplhamapResolution { get; private set; }
		public int Length { get; private set; }
		public int Height { get; private set; }

		public ChunkSettings (int _HeightRes, int _AlphaRes, int _Length, int _Height)
		{
			this.HeightmapResolution = _HeightRes;
			this.AplhamapResolution = _AlphaRes;
			this.Length = _Length;
			this.Height = _Height;
		}
	}
}


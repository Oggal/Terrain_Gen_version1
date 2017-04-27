using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
public class TerrainController
{

    private int Seed;
    private byte octaveFequencyScale;

	private byte octaves;
	private TerrainNoise[] Noise;


	public TerrainController()
	{
		octaves = 4;
        octaveFequencyScale = 5;
        Seed = 99999;
	}

	public void Test()
	{
        PrepNoise();
		for(int x = -5; x < 6; x++)
		{
			for(int y = -5; y < 6; y++)
			{
				TerainGenv2 TG = new TerainGenv2(this, x, y);
				TG.CreateTerrain();
			}
		}
	
	}

    public float getHeight(float x, float y)
    {
        float h = 0;
        foreach(var a in Noise)
        {
            h += a.getHeight(x, y);
        }
        return h;
    }

    private void PrepNoise()
    {
        Noise = new TerrainNoise[octaves];
        for(byte a = 0; a < octaves; a++)
        {
            Noise[a] = new TerrainNoise(Seed, (a + 1) * octaveFequencyScale);
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


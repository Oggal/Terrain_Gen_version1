using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
public class TerrainController
{

    private int Seed;
    private byte octaveFequencyScale;
	public float OcativeWeight = 0.1f;

	private byte octaves;
	private TerrainNoise[]	Noise;
	private TerrainNoise GlobalScale;

	public TerrainController()
	{
		octaves = 4;
        octaveFequencyScale = 25;
        Seed = 99999;
	}

	public void Test()
	{
        PrepNoise();
		for(int x = -1; x < 2; x++)
		{
			for(int y = -1; y < 2; y++)
			{
				TerainGenv2 TG = new TerainGenv2(this, x, y);
				TG.CreateTerrain();
			}
		}
	
	}

    public float getHeight(float x, float y)
    {
        float h = Noise[0].getHeight(x, y)*2;
        for(int j = 1;j<octaves;j++)
        {
			var a = Noise[j];
            h += a.getHeight(x, y) * (OcativeWeight * (j+1));
        }
		h = h * (float)Math.Max(GlobalScale.getHeight(x * 0.001f, y * 0.001f), -0.05);
        return h;
    }

    private void PrepNoise()
    {
        Noise = new TerrainNoise[octaves];
        for(byte a = 0; a < octaves; a++)
        {
            Noise[a] = new TerrainNoise(Seed * a, (a + 1) * octaveFequencyScale);
        }
		GlobalScale = new TerrainNoise(Seed / 2,1);
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


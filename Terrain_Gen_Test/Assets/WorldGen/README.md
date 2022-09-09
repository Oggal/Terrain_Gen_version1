Oggal's World Generator

TerrainNoise.cs
    This is the core noise file for the terrain generator, primarily an attmept at recreating perlin noise.
    The goal is to keep this file free from any unity dependencies.

WorldGen.cs
    This is the primary unity component for the terrain generator. It has a custom inspector, Editor/WorldGenInspector.cs.
    WorldGen.cs contains a number of members to control the generation of terrain.
    Members:
        public int MaxLOD [= 0]

        public bool USeSeed [= true]
            If false    will generate Seed on build
            If true     will use Seed without regenerating
        
        public int Seed [= 0]
            used to Seed primary random
        
        public bool BuildOnStart [ = true]
            if false    will NOT build world on Start
            if true     will build world on Start

        public GameObject Player
            GameObject tracked to keep world centered

        public int OctaveCount [= 4]

        public int PlysOctaveCount [=4]

        public Material[] mats

        public bool UseManyMats [= false]

        public float SmoothingX [= 100]
        public float SmoothingY [= 100]

        private int localX [= 0]
        private int localZ [= 0]
            Represent the number of steps along X and Z from true center (0,0)

        public int TileSize [= 100]
            Size of the phyicical Tile
            This is the true size of the tile in unity units.
        
        public int V_VertexCount [= 100 [3-200]]

        public int P_VertexCount [= 100 [3-200]]

        public int Radius [0-10]
            Radius of Tiles beyond center tile

    Events
        public WorldGenStart
            Called at Start of Inital World Gen
        public WorldGenFinish
            Called at End of TILE gen --NOT INTENDED BEHAVIOR

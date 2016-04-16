using System;

public class VoxelMap
{
    // 32*32*32*10*10
//    private Voxel[] _world = new Voxel[32*32*32*10*10];
//    private Voxel[] _world = new Voxel[1];
    private Voxel[] _world;// = new Voxel[3*3];
    private Random _rnd;

    public int Dimensions
    {
        get { return 320; }
    }

    public int Height
    {
        get { return 32; }
    }

    public Voxel[] World { get { return _world; } }

    public VoxelMap()
    {
        _world = new Voxel[32 * 32 * 32 * 10 * 10];
        _rnd = new Random(511);
        for (int y = 0; y < 3; y++)
        {
            for (int z = 0; z < Dimensions; z++)
            {
                for (int x = 0; x < Dimensions; x++)
                {
                    _world[y * Dimensions * Dimensions + z * Dimensions + x] = new Voxel
                    {
                        PointAA = 1, // y == 2 ? (byte)(_rnd.Next(0, 5)) : (byte)1,
                        PointAB = y == 2 ? (byte)(_rnd.Next(0, 5)) : (byte)1,
                        PointBA = y == 2 ? (byte)(_rnd.Next(0, 5)) : (byte)1,
                        PointBB = 1, //y == 2 ? (byte)(_rnd.Next(0, 5)) : (byte)1
                    };
                }
            }
        }
    }

    public Voxel GetVoxel(int x, int y, int z)
    {
        if (x < 0 || x >= Dimensions || y < 0 || y >= Height || z < 0 || z >= Dimensions) 
            return default(Voxel);
        return _world[y * Dimensions * Dimensions + z * Dimensions + x];
    }

    public Voxel[] GetNeighbors(int x, int y, int z)
    {
        return new[]
        {
            GetVoxel(x, y, z - 1), // North,
            GetVoxel(x, y, z + 1), //South,
            GetVoxel(x + 1, y, z), //East,
            GetVoxel(x - 1, y, z), //West,
            GetVoxel(x, y + 1, z), //Top,
            GetVoxel(x, y - 1, z), //Down
        };
    }
}
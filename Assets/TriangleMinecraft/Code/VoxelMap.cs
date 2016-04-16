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
        get { return 4; }
    }

    public VoxelMap()
    {
        _world = new Voxel[Dimensions * Dimensions];
//        _rnd = new Random();


        for (int z = 0; z < Dimensions; z++)
        {
            for (int x = 0; x < Dimensions; x++)
            {
                _world[z * Dimensions + x] = new Voxel
                {
                    PointAA = 1,
                    PointAB = 1,
                    PointBA = 1,
                    PointBB = 1
                };
            }
        }
        _world[4].PointAB = 0;
        _world[5]  = new Voxel(){ PointAA = 1, PointAB = 0, PointBA = 0, PointBB = 0};
        _world[6]  = new Voxel(){ PointAA = 1, PointAB = 0, PointBA = 0, PointBB = 0 };
        _world[7].PointBA = 0;
        _world[8].PointAB = 0;
        _world[9]  = new Voxel(){ PointAA = 0, PointAB = 0, PointBA = 0, PointBB = 1 };
        _world[10] = new Voxel(){ PointAA = 0, PointAB = 0, PointBA = 0, PointBB = 1 };
        _world[11].PointBA = 0;
    }

    public Voxel GetVoxel(int x, int y, int z)
    {
        return _world[y * Dimensions * Dimensions + z * Dimensions + x];
    }
}
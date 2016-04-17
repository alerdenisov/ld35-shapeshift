//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using Extensions;
//using UnityEngine;
//
//namespace ShapeTetris.Behaviours.Rendering
//{
//
//    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
//    public struct BlockType
//    {
//        public byte Serial;
//        public byte Sub;
//
//        public override bool Equals(object o)
//        {
//            if (o == null) return false;
//            if (o.GetType() != typeof(BlockType)) return false;
//            if (((BlockType)o).Serial != Serial) return false;
//            if (((BlockType)o).Sub != Sub) return false;
//            return true;
//        }
//
//        // Equality operator. Returns dbNull if either operand is dbNull, 
//        // otherwise returns dbTrue or dbFalse:
//        public static bool operator ==(BlockType x, BlockType y)
//        {
//            return x.Equals(y);
//        }
//
//        // Inequality operator. Returns dbNull if either operand is
//        // dbNull, otherwise returns dbTrue or dbFalse:
//        public static bool operator !=(BlockType x, BlockType y)
//        {
//            return !x.Equals(y);
//        }
//
//        public static BlockType Air
//        {
//            get { return new BlockType { Serial = 0x0, Sub = 0x0 }; }
//        }
//
//        public static BlockType Dirt
//        {
//            get
//            {
//                return new BlockType
//                {
//                    Serial = 168,
//                    Sub = 0x0
//                };
//            }
//        }
//
//        public static BlockType WetDirt
//        {
//            get { return new BlockType { Serial = 0x1, Sub = 0x1 }; }
//        }
//    }
//
//    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
//    public class BlockFaceVertex
//    {
//        //        public byte Sunlight;
//
//        // ambient
//
//        // byte mask
//        // XX 00 00 00 - XX A2 A1 A0
//        // 00 - normal
//        // 01 - dark
//        // 11 - light
//
//        private byte _ambient;
//
//        public BlockFace Owner;
//
//        public int AmbientSideA { get { return GetAmbient(0); } }
//        public int AmbientSideB { get { return GetAmbient(1); } }
//        public int AmbientCorner { get { return GetAmbient(2); } }
//
//        public void SetAmbient(int index, bool dark)
//        {
//            SetAmbient(index, dark ? 2 : 1);
//        }
//
//        public int GetAmbient(int index)
//        {
//            var first = _ambient.HasBits(1 << index * 2 + 0);
//            var last = _ambient.HasBits(1 << index * 2 + 1);
//
//            if (first && last) return 1;
//            if (first || last) return -1;
//
//            return 0;
//        }
//
//        public void SetAmbient(int index, int flag)
//        {
//            if (index < 0 || index >= 3) return;
//
//            _ambient = _ambient.SetBit(1 << index * 2 + 0, flag > 0);
//            _ambient = _ambient.SetBit(1 << index * 2 + 1, flag > 1);
//        }
//
//        public int Ambient
//        {
//            get
//            {
//                //                var r = 0;
//                //                if (AmbientSideA) r++;
//                //                if (AmbientSideB) r++;
//                //                if (AmbientCorner) r++;
//
//                //                return (byte)r;
//
//                //                return (AmbientSideA ? 1 : -1) + (AmbientSideB ? 1 : -1) + (AmbientCorner ? 1 : -1);
//
//                return AmbientSideA + AmbientSideB + AmbientCorner;
//
//
//                //                if ((AmbientSideA && AmbientCorner) || (AmbientSideB && AmbientCorner) || (AmbientSideA && AmbientSideB))
//                //                    return 2;
//                //
//                //                if (AmbientSideA || AmbientSideB || AmbientCorner)
//                //                    return 1;
//                //
//                //                return 0;
//            }
//        }
//
//        public BlockFaceVertex(BlockFace owner)
//        {
//            Owner = owner;
//            _ambient = 0;
//        }
//    }
//
//    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
//    public class BlockFace
//    {
//        //        public byte SkyExposure;
//        //        public byte ColorDodge;
//        public BlockFaceVertex[] Vertices;
//        public Block Owner;
//
//        public BlockFace(Block owner)
//        {
//            Owner = owner;
//            CreateVertices();
//        }
//
//        public int Ambient
//        {
//            get
//            {
//                int result = 0;
//                foreach (var vertex in Vertices)
//                    result = result + vertex.Ambient;
//                return result;
//            }
//        }
//
//        private void CreateVertices()
//        {
//            Vertices = new BlockFaceVertex[4];
//            Vertices[0] = new BlockFaceVertex(this);
//            Vertices[1] = new BlockFaceVertex(this);
//            Vertices[2] = new BlockFaceVertex(this);
//            Vertices[3] = new BlockFaceVertex(this);
//        }
//
//    }
//
//    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
//    public class Block
//    {
//        private static Dictionary<BlockType, Block> _blocks;
//
//        public static Dictionary<BlockType, Block> Blocks
//        {
//            get
//            {
//                return _blocks;
//            }
//        }
//
//        static Block()
//        {
//            _blocks = new Dictionary<BlockType, Block>();
//        }
//
//
//        public int Heat { get; private set; }
//        public BlockType Type { get; private set; }
//        public BlockFace[] Faces { get; private set; }
//
//        public bool Transparent
//        {
//            get { return Type == BlockType.Air; }
//        }
//
//        public BlockFace GetFace(Side side)
//        {
//            return Faces[(int)side];
//        }
//
//        public BlockFaceVertex[] GetVertices(Side side)
//        {
//            return GetFace(side).Vertices;
//        }
//
//        public BlockFaceVertex GetVertex(Side side, Corner corner)
//        {
//            return GetFace(side).Vertices[(int)corner];
//        }
//
//        public Block(BlockType type)
//        {
//            Type = type;
//            CreateFaces();
//        }
//
//        private void CreateFaces()
//        {
//            Faces = new BlockFace[6];
//
//            for (int side = 0; side < 6; side++)
//                Faces[side] = new BlockFace(this);
//        }
//
//        public bool IsSimilar(Block other)
//        {
//            return Type == other.Type;
//        }
//
//        public static Block Get(BlockType type)
//        {
//            lock (_blocks)
//            {
//                if (Blocks.ContainsKey(type)) return Blocks[type];
//                else
//                {
//                    var proto = new Block(type);
//                    if (!Blocks.ContainsKey(type)) Blocks.Add(type, proto);
//                    else return Blocks[type];
//                    return proto;
//                }
//            }
//        }
//    }
//
//
//    public enum Side
//    {
//        South = 0,
//        North = 1,
//        East = 2,
//        West = 3,
//        Top = 4,
//        Bottom = 5
//    }
//    public enum Corner
//    {
//        BottomLeft = 0,
//        BottomRight = 1,
//        TopLeft = 2,
//        TopRight = 3
//    }
//
//
//    public static class GreedyBuilder
//    {
//        private static int[] MakeGeneratePositionArray()
//        {
//            var position = new int[3];
//            position[0] = 0;
//            position[1] = 0;
//            position[2] = 0;
//            return position;
//        }
//        
//        private static int[] GetDirectionOffset(int direction)
//        {
//            var offset = new int[3];
//            offset[0] = 0;
//            offset[1] = 0;
//            offset[2] = 0;
//            offset[direction] = 1;
//            return offset;
//        }
//
//        private static Side GetDirectionSide(bool backFace, int direction)
//        {
//            switch (direction)
//            {
//                case 0:
//                    return backFace ? Side.West : Side.East;
//                case 1:
//                    return backFace ? Side.Top : Side.Bottom;
//                case 2:
//                    return backFace ? Side.South : Side.North;
//            }
//
//            throw new System.IndexOutOfRangeException("Wrong direction index");
//        }
//
//        private static Block GetBlock(int[] position, bool getOutOfBox = true)
//        {
//            if (position[0] < 0 || position[0] >= _chunk.Dimension ||
//                position[1] < 0 || position[1] >= _chunk.Dimension ||
//                position[2] < 0 || position[2] >= _chunk.Dimension)
//                return getOutOfBox ? GetEngineBlock(position) : null;
//            else
//                return _chunk[position[0], position[1], position[2]];
//        }
//
//        private static void GenerateDirection(bool backFace, int direction, VoxelData chunk)
//        {
//            var dimensions = new int[]
//            {
//                chunk.Width,
//                chunk.Depth,
//                chunk.Height
//            };
//
//            var horizontal = (direction + 1) % 3;
//            var vertical = (direction + 2) % 3;
//            var position = MakeGeneratePositionArray();
//            var offset = GetDirectionOffset(direction);
//            var side = GetDirectionSide(backFace, direction);
//
//            // goin pass through of chunk by direction
//            for (position[direction] = 0; position[direction] < dimensions[direction]; )
//            {
//                var maskIndex = 0;
//                var mask = new Block[dimensions[direction] * dimensions[direction + 1]];
//
//                for (position[vertical] = 0; position[vertical] < dimensions[direction + 1]; position[vertical]++)
//                {
//                    for (position[horizontal] = 0; position[horizontal] < dimensions[direction]; position[horizontal]++)
//                    {
//                        // calculate culling faces
//                        // get blocks pair for check culling and transparency
//                        var blockA = GetBlock(position);
//                        var blockB = GetBlock(position, offset);
//
//
//                        // check culling
//                        if (blockA == null || blockB == null)
//                            mask[maskIndex] = backFace ? blockA : blockB;
//
//                        maskIndex++;
//                    }
//                }
//
//
//                position[direction]++;
//                maskIndex = 0;
//
//                for (var vertIndex = 0; vertIndex < dimension; vertIndex++)
//                {
//                    for (var horzIndex = 0; horzIndex < dimension; )
//                    {
//                        if (mask[maskIndex] != null)
//                        {
//                            //                                We  compute the width
//                            int width = 1;
//                            int height = 1;
//
//                            for (width = 1;
//                                horzIndex + width < dimension &&
//                                mask[maskIndex + width] != null &&
//                                mask[maskIndex + width].IsSimilar(mask[maskIndex]);
//                                width++)
//                            {
//                            }
//                            //                            //                                    Then we compute height
//                            var done = false;
//                            for (height = 1; vertIndex + height < dimension; height++)
//                            {
//                                for (var line = 0; line < width; line++)
//                                {
//                                    var item = mask[maskIndex + line + height * dimension];
//
//                                    if (item == null || !item.IsSimilar(mask[maskIndex]))
//                                    {
//                                        done = true;
//                                        break;
//                                    }
//                                }
//
//                                if (done)
//                                {
//                                    break;
//                                }
//                            }
//
//                            var du = new[] { 0, 0, 0 };
//                            var dv = new[] { 0, 0, 0 };
//
//
//                            if (!mask[maskIndex].Transparent)
//                            {
//                                position[horizontal] = horzIndex;
//                                position[vertical] = vertIndex;
//
//                                du[0] = 0;
//                                du[1] = 0;
//                                du[2] = 0;
//                                du[horizontal] = width;
//
//                                dv[0] = 0;
//                                dv[1] = 0;
//                                dv[2] = 0;
//                                dv[vertical] = height;
//
//                                MakeQuad(new Vector3(position[0], position[1], position[2]),
//                                         new Vector3(position[0] + du[0], position[1] + du[1], position[2] + du[2]),
//                                         new Vector3(position[0] + du[0] + dv[0], position[1] + du[1] + dv[1], position[2] + du[2] + dv[2]),
//                                         new Vector3(position[0] + dv[0], position[1] + dv[1], position[2] + dv[2]),
//                                         width, height, mask[maskIndex].GetFace(side), backFace);
//
//                                MakeNormals(side);
//                            }
//
//
//                            for (var l = 0; l < height; l++)
//                            {
//                                for (var line = 0; line < width; line++)
//                                {
//                                    mask[maskIndex + line + l * dimension] = null;
//                                }
//                            }
//
//
//                            horzIndex += width;
//                            maskIndex += width;
//                        }
//                        else
//                        {
//                            horzIndex++;
//                            maskIndex++;
//                        }
//                    }
//                }
//            }
//        }
//
//        public static ThreadableMesh Build(VoxelData voxels)
//        {
//            for (bool backFace = true, b = false; b != backFace; backFace = backFace && b, b = !b)
//            {
//                for (int direction = 0; direction < 3; direction++)
//                {
//                    // ignore bottom side (because we never will see that)
//                    if (!backFace && direction == 1) continue;
//                    GenerateDirection(backFace, direction, voxels);
//                }
//            }
//        }
//    }
//}
namespace TriangleMinecraft.Render
{
    public enum FaceCulling : byte
    {
        None            = 0,
        Foot            = 1 << 0,
        Face            = 1 << 1, 
        Right           = 1 << 2,
        Left            = 1 << 3,

        All             = Foot | Face | Right | Left
    }
}
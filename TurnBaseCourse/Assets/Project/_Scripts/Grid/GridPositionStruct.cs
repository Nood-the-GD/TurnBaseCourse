
public struct GridPositionStruct 
{
    public int X;
    public int Z;

    public GridPositionStruct(int x, int z)
    {
        this.X = x;
        this.Z = z;
    }

    public override string ToString()
    {
        return $"x: {X}, z: {Z}";
    }
}

namespace First3D;

public interface IBasis3D
{
    int Size { get; }
    double GetPsi(int number, Point3D point);
    double GetDPsi(int number, int varnumber, Point3D point);
}

public readonly record struct TriQuadraticBasis : IBasis3D
{
    public int Size => 27;

    public double GetPsi(int number, Point3D point)
        => number switch
        {
            0 => GetXi(0, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z),
            1 => GetXi(1, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z), 
            2 => GetXi(2, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z),
            3 => GetXi(0, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z),
            4 => GetXi(1, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z), 
            5 => GetXi(2, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z),
            6 => GetXi(0, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z),
            7 => GetXi(1, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z), 
            8 => GetXi(2, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z),
            9 => GetXi(0, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z),
            10 => GetXi(1, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z), 
            11 => GetXi(2, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z),
            12 => GetXi(0, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z),
            13 => GetXi(1, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z), 
            14 => GetXi(2, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z),
            15 => GetXi(0, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z),
            16 => GetXi(1, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z), 
            17 => GetXi(2, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z),
            18 => GetXi(0, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z),
            19 => GetXi(1, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z), 
            20 => GetXi(2, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z),
            21 => GetXi(0, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z),
            22 => GetXi(1, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z), 
            23 => GetXi(2, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z),
            24 => GetXi(0, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z),
            25 => GetXi(1, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z), 
            26 => GetXi(2, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z),
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected function number")
        };

    public double GetDPsi(int number, int varnumber, Point3D point)
        => varnumber switch
        {
            0 => number switch
            {
                0 => GetDXi(0, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z),
                1 => GetDXi(1, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z), 
                2 => GetDXi(2, point.X) * GetXi(0, point.Y) * GetXi(0, point.Z),
                3 => GetDXi(0, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z),
                4 => GetDXi(1, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z), 
                5 => GetDXi(2, point.X) * GetXi(1, point.Y) * GetXi(0, point.Z),
                6 => GetDXi(0, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z),
                7 => GetDXi(1, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z), 
                8 => GetDXi(2, point.X) * GetXi(2, point.Y) * GetXi(0, point.Z),
                9 => GetDXi(0, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z),
                10 => GetDXi(1, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z), 
                11 => GetDXi(2, point.X) * GetXi(0, point.Y) * GetXi(1, point.Z),
                12 => GetDXi(0, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z),
                13 => GetDXi(1, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z), 
                14 => GetDXi(2, point.X) * GetXi(1, point.Y) * GetXi(1, point.Z),
                15 => GetDXi(0, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z),
                16 => GetDXi(1, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z), 
                17 => GetDXi(2, point.X) * GetXi(2, point.Y) * GetXi(1, point.Z),
                18 => GetDXi(0, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z),
                19 => GetDXi(1, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z), 
                20 => GetDXi(2, point.X) * GetXi(0, point.Y) * GetXi(2, point.Z),
                21 => GetDXi(0, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z),
                22 => GetDXi(1, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z), 
                23 => GetDXi(2, point.X) * GetXi(1, point.Y) * GetXi(2, point.Z),
                24 => GetDXi(0, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z),
                25 => GetDXi(1, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z), 
                26 => GetDXi(2, point.X) * GetXi(2, point.Y) * GetXi(2, point.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected function number")
            },

            1 => number switch
            {
                0 => GetXi(0, point.X) * GetDXi(0, point.Y) * GetXi(0, point.Z),
                1 => GetXi(1, point.X) * GetDXi(0, point.Y) * GetXi(0, point.Z), 
                2 => GetXi(2, point.X) * GetDXi(0, point.Y) * GetXi(0, point.Z),
                3 => GetXi(0, point.X) * GetDXi(1, point.Y) * GetXi(0, point.Z),
                4 => GetXi(1, point.X) * GetDXi(1, point.Y) * GetXi(0, point.Z), 
                5 => GetXi(2, point.X) * GetDXi(1, point.Y) * GetXi(0, point.Z),
                6 => GetXi(0, point.X) * GetDXi(2, point.Y) * GetXi(0, point.Z),
                7 => GetXi(1, point.X) * GetDXi(2, point.Y) * GetXi(0, point.Z), 
                8 => GetXi(2, point.X) * GetDXi(2, point.Y) * GetXi(0, point.Z),
                9 => GetXi(0, point.X) * GetDXi(0, point.Y) * GetXi(1, point.Z),
                10 => GetXi(1, point.X) * GetDXi(0, point.Y) * GetXi(1, point.Z), 
                11 => GetXi(2, point.X) * GetDXi(0, point.Y) * GetXi(1, point.Z),
                12 => GetXi(0, point.X) * GetDXi(1, point.Y) * GetXi(1, point.Z),
                13 => GetXi(1, point.X) * GetDXi(1, point.Y) * GetXi(1, point.Z), 
                14 => GetXi(2, point.X) * GetDXi(1, point.Y) * GetXi(1, point.Z),
                15 => GetXi(0, point.X) * GetDXi(2, point.Y) * GetXi(1, point.Z),
                16 => GetXi(1, point.X) * GetDXi(2, point.Y) * GetXi(1, point.Z), 
                17 => GetXi(2, point.X) * GetDXi(2, point.Y) * GetXi(1, point.Z),
                18 => GetXi(0, point.X) * GetDXi(0, point.Y) * GetXi(2, point.Z),
                19 => GetXi(1, point.X) * GetDXi(0, point.Y) * GetXi(2, point.Z), 
                20 => GetXi(2, point.X) * GetDXi(0, point.Y) * GetXi(2, point.Z),
                21 => GetXi(0, point.X) * GetDXi(1, point.Y) * GetXi(2, point.Z),
                22 => GetXi(1, point.X) * GetDXi(1, point.Y) * GetXi(2, point.Z), 
                23 => GetXi(2, point.X) * GetDXi(1, point.Y) * GetXi(2, point.Z),
                24 => GetXi(0, point.X) * GetDXi(2, point.Y) * GetXi(2, point.Z),
                25 => GetXi(1, point.X) * GetDXi(2, point.Y) * GetXi(2, point.Z), 
                26 => GetXi(2, point.X) * GetDXi(2, point.Y) * GetXi(2, point.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected function number")
            },
            
            2 => number switch
            {
                0 => GetXi(0, point.X) * GetXi(0, point.Y) * GetDXi(0, point.Z),
                1 => GetXi(1, point.X) * GetXi(0, point.Y) * GetDXi(0, point.Z), 
                2 => GetXi(2, point.X) * GetXi(0, point.Y) * GetDXi(0, point.Z),
                3 => GetXi(0, point.X) * GetXi(1, point.Y) * GetDXi(0, point.Z),
                4 => GetXi(1, point.X) * GetXi(1, point.Y) * GetDXi(0, point.Z), 
                5 => GetXi(2, point.X) * GetXi(1, point.Y) * GetDXi(0, point.Z),
                6 => GetXi(0, point.X) * GetXi(2, point.Y) * GetDXi(0, point.Z),
                7 => GetXi(1, point.X) * GetXi(2, point.Y) * GetDXi(0, point.Z), 
                8 => GetXi(2, point.X) * GetXi(2, point.Y) * GetDXi(0, point.Z),
                9 => GetXi(0, point.X) * GetXi(0, point.Y) * GetDXi(1, point.Z),
                10 => GetXi(1, point.X) * GetXi(0, point.Y) * GetDXi(1, point.Z), 
                11 => GetXi(2, point.X) * GetXi(0, point.Y) * GetDXi(1, point.Z),
                12 => GetXi(0, point.X) * GetXi(1, point.Y) * GetDXi(1, point.Z),
                13 => GetXi(1, point.X) * GetXi(1, point.Y) * GetDXi(1, point.Z), 
                14 => GetXi(2, point.X) * GetXi(1, point.Y) * GetDXi(1, point.Z),
                15 => GetXi(0, point.X) * GetXi(2, point.Y) * GetDXi(1, point.Z),
                16 => GetXi(1, point.X) * GetXi(2, point.Y) * GetDXi(1, point.Z), 
                17 => GetXi(2, point.X) * GetXi(2, point.Y) * GetDXi(1, point.Z),
                18 => GetXi(0, point.X) * GetXi(0, point.Y) * GetDXi(2, point.Z),
                19 => GetXi(1, point.X) * GetXi(0, point.Y) * GetDXi(2, point.Z), 
                20 => GetXi(2, point.X) * GetXi(0, point.Y) * GetDXi(2, point.Z),
                21 => GetXi(0, point.X) * GetXi(1, point.Y) * GetDXi(2, point.Z),
                22 => GetXi(1, point.X) * GetXi(1, point.Y) * GetDXi(2, point.Z), 
                23 => GetXi(2, point.X) * GetXi(1, point.Y) * GetDXi(2, point.Z),
                24 => GetXi(0, point.X) * GetXi(2, point.Y) * GetDXi(2, point.Z),
                25 => GetXi(1, point.X) * GetXi(2, point.Y) * GetDXi(2, point.Z), 
                26 => GetXi(2, point.X) * GetXi(2, point.Y) * GetDXi(2, point.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected function number")
            },
            
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected function number")
        };

    private double GetXi(int number, double value)
        => number switch
        {
            0 => 2 * (value - 0.5) * (value - 1),
            1 => -4 * value * (value - 1),
            2 => 2 * value * (value - 0.5),
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected Xi member")
        };

    private double GetDXi(int number, double value)
        => number switch
        {
            0 => 4 * (value - 0.75),
            1 => 4 - 8 * value,
            2 => 4 * (value - 0.25),
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Not expected DXi member")
        };
}
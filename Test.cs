namespace First3D;

public abstract class Test
{
    protected double lambda;
    protected double sigma;

    public Test(Grid grid)
    {
        lambda = grid.Lambda;
        sigma = grid.Sigma;
    }
    
    public abstract double U(Point3D point, double t);
    
    public abstract double F(Point3D point, double t);
}

public class Test1 : Test
{
    public Test1(Grid grid) : base(grid) { }
    
    public override double U(Point3D point, double t)
        => point.X + point.Y + point.Z + t;

    public override double F(Point3D point, double t)
        => 1;
}
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

    public abstract double Theta(Point3D point, double t, ElementSide elementSide);
}

public class Test1 : Test
{
    public Test1(Grid grid) : base(grid) { }
    
    public override double U(Point3D point, double t)
        => point.Y + 2 * point.Z + t;

    public override double F(Point3D point, double t)
        => 1;

    public override double Theta(Point3D point, double t, ElementSide elementSide)
    {
        switch (elementSide)
        {
            case ElementSide.Left:
            case ElementSide.Right:
                return 0;
            
            case ElementSide.Bottom:
            case ElementSide.Upper:
                return 2;
            
            case ElementSide.Rear:
            case ElementSide.Front:
                return 1;
            
            default:
                throw new Exception("Can't find function type");
        }
    }
}
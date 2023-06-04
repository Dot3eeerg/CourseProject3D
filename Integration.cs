namespace First3D;

public class Integration
{
    private readonly IEnumerable<QuadratureNode> _quadratures;

    public Integration(IEnumerable<QuadratureNode> quadratures) => _quadratures = quadratures;

    public double Gauss3D(Func<Point3D, double> psi)
    {
        double result = 0;
        Point3D point = new(0, 0, 0);

        foreach (var qi in _quadratures)
        {
            point.X = (qi.Node + 1) / 2.0;
            
            foreach (var qj in _quadratures)
            {
                point.Y = (qj.Node + 1) / 2.0;
                
                foreach (var qk in _quadratures)
                {
                    point.Z = (qk.Node + 1) / 2.0;

                    result += psi(point) * qi.Weight * qj.Weight * qk.Weight;
                }
            }
        }

        return result / 8.0;
    }
}
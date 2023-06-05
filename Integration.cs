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
    
    public double Gauss2D(Func<Point3D, double> psi, ElementSide elementSide)
    {
        double result = 0;
        Point3D point = new(0, 0, 0);

        switch (elementSide)
        {
            case ElementSide.Left:
                point.X = 0;
                foreach (var qi in _quadratures)
                {
                    point.Y = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Z = (qj.Node + 1) / 2.0;

                        result += -psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
            
            case ElementSide.Right:
                point.X = 1;
                foreach (var qi in _quadratures)
                {
                    point.Y = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Z = (qj.Node + 1) / 2.0;

                        result += psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
            
            case ElementSide.Bottom:
                point.Z = 0;
                foreach (var qi in _quadratures)
                {
                    point.X = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Y = (qj.Node + 1) / 2.0;

                        result += -psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
            
            case ElementSide.Upper:
                point.Z = 1;
                foreach (var qi in _quadratures)
                {
                    point.X = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Y = (qj.Node + 1) / 2.0;

                        result += psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
            
            case ElementSide.Rear:
                point.Y = 1;
                foreach (var qi in _quadratures)
                {
                    point.X = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Z = (qj.Node + 1) / 2.0;

                        result += psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
            
            case ElementSide.Front:
                point.Y = 0;
                foreach (var qi in _quadratures)
                {
                    point.X = (qi.Node + 1) / 2.0;

                    foreach (var qj in _quadratures)
                    {
                        point.Z = (qj.Node + 1) / 2.0;

                        result += -psi(point) * qi.Weight * qj.Weight;
                    }
                }
                break;
        }

        return result / 4.0;
    }
}
namespace First3D;

public class QuadratureNode
{
    public double Node { get; }
    public double Weight { get; }

    public QuadratureNode(double node, double weight)
    {
        Node = node;
        Weight = weight;
    }
}

public static class Quadratures
{
    public static IEnumerable<QuadratureNode> SegmentGaussOrder9()
    {
        const int n = 5;
        
        double[] points =
        {
            0.0,
            1.0 / 3.0 * Math.Sqrt(5 - 2 * Math.Sqrt(10.0 / 7.0)),
            -1.0 / 3.0 * Math.Sqrt(5 - 2 * Math.Sqrt(10.0 / 7.0)),
            1.0 / 3.0 * Math.Sqrt(5 + 2 * Math.Sqrt(10.0 / 7.0)),
            -1.0 / 3.0 * Math.Sqrt(5 + 2 * Math.Sqrt(10.0 / 7.0))
        };

        double[] weights =
        {
            128.0 / 225.0,
            (322.0 + 13.0 * Math.Sqrt(70.0)) / 900.0,
            (322.0 + 13.0 * Math.Sqrt(70.0)) / 900.0,
            (322.0 - 13.0 * Math.Sqrt(70.0)) / 900.0,
            (322.0 - 13.0 * Math.Sqrt(70.0)) / 900.0
        };

        for (int i = 0; i < n; i++)
            yield return new(points[i], weights[i]);
    }
}
namespace CourseProjetc3D;

public class Grid
{
    private readonly double _xStart;
    private readonly double _xEnd;
    private readonly int _xSteps;
    private readonly double _xRaz;
    private readonly double _yStart;
    private readonly double _yEnd;
    private readonly int _ySteps;
    private readonly double _yRaz;
    private readonly double _zStart;
    private readonly double _zEnd;
    private readonly int _zSteps;
    private readonly double _zRaz;
    private readonly int[] _boundaries;
    public Point3D[] Nodes { get; private set; }
    public HashSet<int> DirichletBoundaries { get; private set; } 
    public HashSet<int> NewmanBoundaries { get; private set; } 
    public int[][] Elements { get; private set; }
    public double Lambda { get; set; }
    public double Sigma { get; set; }

    public Grid(string path)
    {
        using (var sr = new StreamReader(path))
        {
            string[] data;
            data = sr.ReadLine()!.Split(" ").ToArray();
            _xStart = Convert.ToDouble(data[0]);
            _xEnd = Convert.ToDouble(data[1]);
            _xSteps = Convert.ToInt32(data[2]);
            _xRaz = Convert.ToDouble(data[3]);
            
            data = sr.ReadLine()!.Split(" ").ToArray();
            _yStart = Convert.ToDouble(data[0]);
            _yEnd = Convert.ToDouble(data[1]);
            _ySteps = Convert.ToInt32(data[2]);
            _yRaz = Convert.ToDouble(data[3]);
            
            data = sr.ReadLine()!.Split(" ").ToArray();
            _zStart = Convert.ToDouble(data[0]);
            _zEnd = Convert.ToDouble(data[1]);
            _zSteps = Convert.ToInt32(data[2]);
            _zRaz = Convert.ToDouble(data[3]);

            data = sr.ReadLine()!.Split(" ").ToArray();
            Lambda = Convert.ToDouble(data[0]);
            Sigma = Convert.ToDouble(data[1]);

            data = sr.ReadLine()!.Split(" ").ToArray();
            _boundaries = new int[6];
            _boundaries[0] = Convert.ToInt32(data[0]);
            _boundaries[1] = Convert.ToInt32(data[1]);
            _boundaries[2] = Convert.ToInt32(data[2]);
            _boundaries[3] = Convert.ToInt32(data[3]);
            _boundaries[4] = Convert.ToInt32(data[4]);
            _boundaries[5] = Convert.ToInt32(data[5]);
        }
    }

    public void BuildGrid()
    {
        Elements = new int[_xSteps * _ySteps * _zSteps].Select(_ => new int[27]).ToArray();
        Nodes = new Point3D[(_xSteps * 2 + 1) * (_ySteps * 2 + 1) * (_zSteps * 2 + 1)];

        double sumRazX = 0, sumRazY = 0, sumRazZ = 0;
        for (int i = 0; i < _xSteps; i++)
            sumRazX += Math.Pow(_xRaz, i);
        
        for (int i = 0; i < _ySteps; i++)
            sumRazY += Math.Pow(_yRaz, i);

        for (int i = 0; i < _zSteps; i++)
            sumRazZ += Math.Pow(_zRaz, i);

        int nodesInRow = _xSteps * 2 + 1;
        int nodesInSlice = nodesInRow * (_ySteps * 2 + 1);

        double x = _xStart, y = _yStart, z = _zStart;
        double xStep = (_xEnd - _xStart) / sumRazX;
        double yStep = (_yEnd - _yStart) / sumRazY;
        double zStep = (_zEnd - _zStart) / sumRazZ;

        DirichletBoundaries = new();
        NewmanBoundaries = new();

        for (int j = 0; j < _xSteps * 2; j+=2)
        {
            Nodes[j] = new(x, y, z);
            x += xStep / 2;
            Nodes[j + 1] = new(x, y, z);
            x += xStep / 2;
            xStep *= _xRaz;
            if (_boundaries[2] == 1)
            {
                DirichletBoundaries.Add(j);
                DirichletBoundaries.Add(j + 1);
            }
            else
            {
                NewmanBoundaries.Add(j);
                NewmanBoundaries.Add(j + 1);
            }
        }

        Nodes[_xSteps * 2] = new(_xEnd, y, z);
        if (_boundaries[2] == 1) DirichletBoundaries.Add(_xSteps * 2);
        else NewmanBoundaries.Add(_xSteps * 2);

        for (int i = 1; i < _ySteps * 2; i+=2)
        {
            y += yStep / 2;
            for (int j = 0; j < _xSteps * 2 + 1; j++)
            {
                Nodes[i * nodesInRow + j] = new(Nodes[j].X, y, z);
                if (_boundaries[2] == 1) DirichletBoundaries.Add(i * nodesInRow + j);
                else NewmanBoundaries.Add(i * nodesInRow + j);
            }

            y += yStep / 2;
            yStep *= _yRaz;
            for (int j = 0; j < _xSteps * 2 + 1; j++)
            {
                Nodes[(i + 1) * nodesInRow + j] = new(Nodes[j].X, y, z);
                if (_boundaries[2] == 1) DirichletBoundaries.Add((i + 1) * nodesInRow + j);
                else NewmanBoundaries.Add((i + 1) * nodesInRow + j);
            }
        }
        
        //for (int j = 0; j < _xSteps * 2 + 1; j++)
        //{
        //    Nodes[(_ySteps + 2) * nodesInRow + j] = new(Nodes[j].X, _yEnd, z);
        //    if (_boundaries[2] == 1) DirichletBoundaries.Add((_ySteps + 2) * nodesInRow + j);
        //    else NewmanBoundaries.Add((_ySteps + 2) * nodesInRow + j);
        //}

        for (int i = 1; i < _zSteps; i++)
        {
            z += zStep;
            zStep *= _zRaz;
            for (int j = 0; j < _ySteps + 1; j++)
            {
                for (int k = 0; k < _zSteps + 1; k++)
                {
                    Nodes[i * nodesInSlice + j * nodesInRow + k] = new(Nodes[k].X, Nodes[j * nodesInRow].Y, z);
                    if (Math.Abs(Nodes[k].X - _xStart) < 1e-12 || Math.Abs(Nodes[k].X - _xEnd) < 1e-12 || Math.Abs(Nodes[j * nodesInRow].Y - _yStart) < 1e-12 ||
                        Math.Abs(Nodes[j * nodesInRow].Y - _yEnd) < 1e-12)
                        DirichletBoundaries.Add(i * nodesInSlice + j * nodesInRow + k);
                }
            }
        }

        for (int j = 0; j < _ySteps + 1; j++)
        {
            for (int k = 0; k < _xSteps + 1; k++)
            {
                Nodes[_zSteps * nodesInSlice + j * nodesInRow + k] = new(Nodes[k].X, Nodes[j * nodesInRow].Y, _zEnd);
                DirichletBoundaries.Add(_zSteps * nodesInSlice + j * nodesInRow + k);
            }
        }

        int index = 0;

        for (int k = 0; k < _zSteps; k++)
        {
            for (int i = 0; i < _ySteps; i++)
            {
                for (int j = 0; j < _xSteps; j++)
                {
                    Elements[index][0] = j + nodesInRow * i + nodesInSlice * k;
                    Elements[index][1] = (j + 1) + nodesInRow * i + nodesInSlice * k;
                    Elements[index][2] = j + nodesInRow * (i + 1) + nodesInSlice * k;
                    Elements[index][3] = (j + 1) + nodesInRow * (i + 1) + nodesInSlice * k;
                    Elements[index][4] = j + nodesInRow * i + nodesInSlice * (k + 1);
                    Elements[index][5] = (j + 1) + nodesInRow * i + nodesInSlice * (k + 1);
                    Elements[index][6] = j + nodesInRow * (i + 1) + nodesInSlice * (k + 1);
                    Elements[index++][7] = (j + 1) + nodesInRow * (i + 1) + nodesInSlice * (k + 1);
                }
            }
        }
    }
}
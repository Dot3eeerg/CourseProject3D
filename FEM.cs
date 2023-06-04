using System.Globalization;

namespace First3D;

public class FEM
{
    private SparseMatrix _globalMatrix = default!;
    private Vector _globalVector = default!;
    private Vector _solution = default!;
    private Vector _localVector = default!;
    private Matrix _stiffnessMatrix;
    private Matrix _massMatrix;
    private Grid _grid;
    private TimeGrid _timeGrid;
    private Vector[] _layers = default!;
    private Test _test = default!;
    private IBasis3D _basis = default!;
    private Integration _integration = default!;

    public FEM(Grid grid, TimeGrid timeGrid)
    {
        _grid = grid;
        _timeGrid = timeGrid;
        _basis = new TriQuadraticBasis();
        _integration = new Integration(Quadratures.SegmentGaussOrder9());
        _stiffnessMatrix = new(_basis.Size);
        _massMatrix = new(_basis.Size);
        _localVector = new(_basis.Size);
    }

    public void SetTest(Test test)
    {
        _test = test;
    }

    public void Compute()
    {
        BuildPortrait();
        PrepareLayers();

        for (int itime = 3; itime < _timeGrid.TGrid.Length; itime++)
        {
            AssemblySLAE(itime);
            AccountDirichletBoundaries(itime);
        }
    }

    private void PrepareLayers()
    {
        int i = 0;
        foreach (var kek in _grid.Nodes)
        {
            _layers[0][i] = _test.U(kek, _timeGrid[0]);
            _layers[1][i] = _test.U(kek, _timeGrid[1]);
            _layers[2][i++] = _test.U(kek, _timeGrid[2]);
        }
    }

    private void AssemblySLAE(int itime)
    {
        _globalVector.Fill(0);
        _globalMatrix.Clear();

        for (int ielem = 0; ielem < _grid.Elements.Length; ielem++)
        {
            AssemblyLocalElement(ielem, itime);

            double t01 = _timeGrid[itime] - _timeGrid[itime - 1];
            double t02 = _timeGrid[itime] - _timeGrid[itime - 2];
            double t03 = _timeGrid[itime] - _timeGrid[itime - 3];

            _stiffnessMatrix += (1 / t03 + 1 / t02 + 1 / t01) * _massMatrix;

            for (int i = 0; i < _basis.Size; i++)
            {
                for (int j = 0; j < _basis.Size; j++)
                {
                    AddElement(_grid.Elements[ielem][i], _grid.Elements[ielem][j], _stiffnessMatrix[i, j]);
                }
            }
            
            AssemblyGlobalVector(ielem, itime);
            
            _stiffnessMatrix.Clear();
            _massMatrix.Clear();
            _localVector.Fill(0);
        }
    }

    private void AssemblyGlobalVector(int ielem, int itime)
    {
        double t01 = _timeGrid[itime] - _timeGrid[itime - 1];
        double t02 = _timeGrid[itime] - _timeGrid[itime - 2];
        double t03 = _timeGrid[itime] - _timeGrid[itime - 3];
        double t12 = _timeGrid[itime - 1] - _timeGrid[itime - 2];
        double t13 = _timeGrid[itime - 1] - _timeGrid[itime - 3];
        double t23 = _timeGrid[itime - 2] - _timeGrid[itime - 3];

        double[] qj1 = new double[_basis.Size];
        double[] qj2 = new double[_basis.Size];
        double[] qj3 = new double[_basis.Size];

        for (int i = 0; i < _basis.Size; i++)
        {
            for (int j = 0; j < _basis.Size; j++)
            {
                qj1[i] += _massMatrix[i, j] * _layers[2][_grid.Elements[ielem][j]];
                qj2[i] += _massMatrix[i, j] * _layers[1][_grid.Elements[ielem][j]];
                qj3[i] += _massMatrix[i, j] * _layers[0][_grid.Elements[ielem][j]];
            }
        }

        for (int i = 0; i < _basis.Size; i++)
        {
            _localVector[i] += qj1[i] * t03 * t02 / (t13 * t12 * t01) - qj2[i] * t03 * t01 / (t23 * t12 * t02) +
                               qj3[i] * t02 * t01 / (t23 * t13 * t03);

            _globalVector[_grid.Elements[ielem][i]] += _localVector[i];
        }
    }

    private void AddElement(int i, int j, double value)
    {
        if (i == j)
        {
            _globalMatrix.Di[i] = value;
            return;
        }

        if (i > j)
        {
            for (int icol = _globalMatrix.Ig[i]; icol < _globalMatrix.Ig[i + 1]; icol++)
            {
                if (_globalMatrix.Jg[icol] == j)
                {
                    _globalMatrix.Ggl[icol] += value;
                    return;
                }
            }
        }

        else
        {
            for (int icol = _globalMatrix.Ig[j]; icol < _globalMatrix.Ig[j + 1]; icol++)
            {
                if (_globalMatrix.Jg[icol] == i)
                {
                    _globalMatrix.Ggu[icol] += value;
                    return;
                }
            }
        }
    }

    private void AssemblyLocalElement(int ielem, int itime)
    {
        double hx = _grid.Nodes[_grid.Elements[ielem][26]].X - _grid.Nodes[_grid.Elements[ielem][0]].X;
        double hy = _grid.Nodes[_grid.Elements[ielem][26]].Y - _grid.Nodes[_grid.Elements[ielem][0]].Y;
        double hz = _grid.Nodes[_grid.Elements[ielem][26]].Z - _grid.Nodes[_grid.Elements[ielem][0]].Z;

        for (int i = 0; i < _basis.Size; i++)
        {
            for (int j = 0; j < _basis.Size; j++)
            {
                Func<Point3D, double> kek;

                int ik = i;
                int jk = j;
                kek = point =>
                {
                    double psi1 = _basis.GetPsi(ik, point);
                    double psi2 = _basis.GetPsi(jk, point);

                    return psi1 * psi2;
                };

                _massMatrix[i, j] = hx * hy * hz * _integration.Gauss3D(kek);


                kek = point =>
                {
                    double dPsi1 = _basis.GetDPsi(ik, 0, point);
                    double dPsi2 = _basis.GetDPsi(jk, 0, point);

                    return dPsi1 * dPsi2;
                };
                _stiffnessMatrix[i, j] = hy * hz / hx * _grid.Lambda * _integration.Gauss3D(kek);

                kek = point =>
                {
                    double dPsi1 = _basis.GetDPsi(ik, 1, point);
                    double dPsi2 = _basis.GetDPsi(jk, 1, point);

                    return dPsi1 * dPsi2;
                };
                _stiffnessMatrix[i, j] += hx * hz / hy * _grid.Lambda * _integration.Gauss3D(kek);
                
                kek = point =>
                {
                    double dPsi1 = _basis.GetDPsi(ik, 2, point);
                    double dPsi2 = _basis.GetDPsi(jk, 2, point);

                    return dPsi1 * dPsi2;
                };
                _stiffnessMatrix[i, j] += hy * hx / hz * _grid.Lambda * _integration.Gauss3D(kek);
            }

            _localVector[i] = _test.F(_grid.Nodes[_grid.Elements[ielem][i]], _timeGrid[itime]);
        }

        _localVector = _massMatrix * _localVector;

        _massMatrix = _grid.Sigma * _massMatrix;
    }
    
    private void BuildPortrait()
    {
        HashSet<int>[] list = new HashSet<int>[_grid.Nodes.Length].Select(_ => new HashSet<int>()).ToArray();
        foreach (var element in _grid.Elements)
            foreach (var pos in element)
                foreach (var node in element)
                    if (pos > node)
                        list[pos].Add(node);

        list = list.Select(childlist => childlist.Order().ToHashSet()).ToArray();
        int count = list.Sum(childlist => childlist.Count);

        _globalMatrix = new(_grid.Nodes.Length, count);
        _globalVector = new(_grid.Nodes.Length);
        _layers = new Vector[3].Select(_ => new Vector(_grid.Nodes.Length)).ToArray();

        _globalMatrix.Ig[0] = 0;

        for (int i = 0; i < list.Length; i++)
            _globalMatrix.Ig[i + 1] = _globalMatrix.Ig[i] + list[i].Count;

        int k = 0;

        foreach (var childlist in list)
            foreach (var value in childlist)
                _globalMatrix.Jg[k++] = value;
    }

    private void AccountDirichletBoundaries(int itime)
    {
        foreach (var node in _grid.DirichletBoundaries)
        {

            _globalMatrix.Di[node] = 1;
            _globalVector[node] = _test.U(_grid.Nodes[node], _timeGrid[itime]);

            for (int i = _globalMatrix.Ig[node]; i < _globalMatrix.Ig[node + 1]; i++)
                _globalMatrix.Ggl[i] = 0;

            for (int col = node + 1; col < _globalMatrix.Size; col++)
                for (int j = _globalMatrix.Ig[col]; j < _globalMatrix.Ig[col + 1]; j++)
                    if (_globalMatrix.Jg[j] == node)
                    {
                        _globalMatrix.Ggu[j] = 0;
                        break;
                    }
        }
    }
}
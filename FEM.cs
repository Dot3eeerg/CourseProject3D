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
    private Vector[] _layers = default;

    public FEM(Grid grid, TimeGrid timeGrid)
    {
        _grid = grid;
        _timeGrid = timeGrid;
        _stiffnessMatrix = new(27);
        _massMatrix = new(27);
        _localVector = new(27);
    }

    public void Compute()
    {
        BuildPortrait();
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
}
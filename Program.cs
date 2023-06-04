using First3D;

Grid grid = new Grid("GridParameters");
grid.BuildGrid();
grid.AccountBoundaryConditions();

TimeGrid timeGrid = new TimeGrid("TimeGridParameters");
timeGrid.BuildTimeGrid();

FEM fem = new FEM(grid, timeGrid);
fem.SetTest(new Test1(grid));
fem.SetSolver(new LOSLUSolver());
fem.Compute();
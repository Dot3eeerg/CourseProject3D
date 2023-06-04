namespace First3D;

public class Matrix
{
    private double[,] _mat;
    public int Size { get; }

    public double this[int i, int j]
    {
        get => _mat[i, j];
        set => _mat[i, j] = value;
    }

    public Matrix(int size)
    {
        _mat = new double[size, size];
        Size = size;
    }

    public void Clear()
        => Array.Clear(_mat, 0, _mat.Length);

    public void Copy(Matrix destination)
    {
        for (int i = 0; i < destination.Size; i++)
        {
            for (int j = 0; j < destination.Size; j++)
            {
                destination[i, j] = _mat[i, j];
            }
        }
    }

    public static Matrix operator +(Matrix fstMatrix, Matrix sndMatrix)
    {
        Matrix resultMatrix = new(fstMatrix.Size);

        for (int i = 0; i < resultMatrix.Size; i++)
        {
            for (int j = 0; j < resultMatrix.Size; j++)
            {
                resultMatrix[i, j] = fstMatrix[i, j] + sndMatrix[i, j];
            }
        }

        return resultMatrix;
    }
   
    public static Matrix operator *(double coef, Matrix Mat)
    {
        Matrix resultMatrix = new(Mat.Size);

        for (int i = 0; i < resultMatrix.Size; i++)
        {
            for (int j = 0; j < resultMatrix.Size; j++)
            {
                resultMatrix[i, j] = coef * Mat[i, j];
            }
        }

        return resultMatrix;
    }
}
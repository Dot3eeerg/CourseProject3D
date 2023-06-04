namespace First3D;

public class Vector
{
   private double[] _vec;
   public int Length { get; }

   public Vector(int dim)
   {
      _vec = new double[dim];
      Length = dim;
   }

   public double this[int index]
   {
      get => _vec[index];
      set => _vec[index] = value;
   }

   public void Randomize()
   {
      for (int i = 0; i < Length; i++)
         _vec[i] = new Random().Next(1, 10);
   }

   public void Fill(double value)
   {
      for (int i = 0; i < Length; i++)
      {
         _vec[i] = value;
      }
   }

   public static void Copy(Vector source, Vector destination)
   {
      for (int i = 0; i < source.Length; i++)
         destination[i] = source[i];
   }

   public void Norming()
   {
      double norm = Norm();

      for (int i = 0; i < Length; i++)
         _vec[i] /= norm;
   }

   public double Norm()
   {
      double result = 0;

      for (int i = 0; i < Length; i++)
         result += _vec[i] * _vec[i];

      return Math.Sqrt(result);
   }

   public double NormFEM()
   {
      double result = 0;

      for (int i = 1; i < Length - 1; i++)
         result += _vec[i] * _vec[i];

      return Math.Sqrt(result);
   }

   public static Vector operator *(Matrix matrix, Vector vector)
   {
      Vector result = new(vector._vec.Length);

      for (int i = 0; i < vector.Length; i++)
         for (int j = 0; j < vector.Length; j++)
            result._vec[i] += matrix[i, j] * vector._vec[j];

      return result;
   }

   public static Vector operator -(Vector fstVector, Vector sndVector)
   {
      Vector result = new(fstVector.Length);

      for (int i = 0; i < fstVector.Length; i++)
         result[i] = fstVector._vec[i] - sndVector._vec[i];

      return result;
   }

   public static Vector operator +(Vector fstVector, Vector sndVector)
   {
      Vector result = new(fstVector.Length);

      for (int i = 0; i < fstVector.Length; i++)
         result[i] = fstVector._vec[i] + sndVector._vec[i];

      return result;
   }

   public static Vector operator *(double coef, Vector vector)
   {
      Vector result = new(vector.Length);

      for (int i = 0; i < vector.Length; i++)
         result[i] = vector._vec[i] * coef;

      return result;
   }

   public static double operator *(Vector fstVector, Vector sndVector)
   {
      double result = 0;

      for (int i = 0; i < fstVector.Length; i++)
         result += fstVector._vec[i] * sndVector._vec[i];

      return result;
   }
}
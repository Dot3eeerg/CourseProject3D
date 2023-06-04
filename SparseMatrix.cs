namespace First3D;

public class SparseMatrix
{
   public int[] Ig { get; set; }
   public int[] Jg { get; set; }
   public double[] Di { get; set; }
   public double[] Ggl { get; set; }
   public double[] Ggu { get; set; }
   public int Size { get; set; }

   public SparseMatrix(int size, int sizeOffDiag)
   {
      Size = size;
      Ig = new int[size + 1];
      Jg = new int[sizeOffDiag];
      Ggl = new double[sizeOffDiag];
      Ggu = new double[sizeOffDiag];
      Di = new double[size];
   }

   public static Vector operator *(SparseMatrix matrix, Vector vector)
   {
      Vector product = new(vector.Length);

      for (int i = 0; i < vector.Length; i++)
      {
         product[i] += matrix.Di[i] * vector[i];

         for (int j = matrix.Ig[i]; j < matrix.Ig[i + 1]; j++)
         {
            product[i] += matrix.Ggl[j] * vector[matrix.Jg[j]];
            product[matrix.Jg[j]] += matrix.Ggu[j] * vector[i];
         }
      }

      return product;
   }

   public void Clear()
   {
      for (int i = 0; i < Size; i++)
      {
         Di[i] = 0.0;

         for (int k = Ig[i]; k < Ig[i + 1]; k++)
         {
            Ggl[k] = 0.0;
            Ggu[k] = 0.0;
         }
      }
   }

}
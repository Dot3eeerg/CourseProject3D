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
   public SparseMatrix ConvertToProfile()
   {
      int sizeOffDiag = 0;
      for (int i = 0; i < Size; i++)
      {
         sizeOffDiag += i - Jg[Ig[i]];
      }

      SparseMatrix result = new(Size, sizeOffDiag);

      result.Ig[0] = 0;

      for (int i = 0; i < Size; i++)
      {
         result.Di[i] = Di[i];

         int rowSize = i - Jg[Ig[i]];
         result.Ig[i + 1] = result.Ig[i] + rowSize;

         int jPrev = Ig[i];
         for (int j = result.Ig[i]; j < result.Ig[i + 1]; j++)
         {
            int col = i - (result.Ig[i + 1] - j);
            int colPrev = jPrev < Ig[i + 1]? Jg[jPrev] : i;
            if (col == colPrev)
            {
               result.Ggl[j] = Ggl[jPrev];
               result.Ggu[j] = Ggu[jPrev++];
            }
            else
            {
               result.Ggl[j] = 0;
               result.Ggu[j] = 0;
            }
            result.Jg[j] = col;
         }
      }

      return result;
   }

   public static void Copy(SparseMatrix source, SparseMatrix product)
   {
      for (int i = 0; i < product.Size + 1; i++)
      {
         product.Ig[i] = source.Ig[i];
      }

      for (int i = 0; i < product.Size; i++)
      {
         product.Di[i] = source.Di[i];
      }

      for (int i = 0; i < product.Jg.Length; i++)
      {
         product.Jg[i] = source.Jg[i];
         product.Ggl[i] = source.Ggl[i];
         product.Ggu[i] = source.Ggu[i];
      }
   }
}
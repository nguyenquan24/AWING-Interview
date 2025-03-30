using System.ComponentModel.DataAnnotations;

namespace TreasureHunt.Core.Models
{
    public class TreasureMapRequest
    {
        [Range(1, 500)]
        public int N { get; set; }

        [Range(1, 500)]
        public int M { get; set; }

        [Range(1, 250000)]
        public int P { get; set; }

        public Dictionary<string, int> Matrix { get; set; } = new();

        // Add validation
        public bool IsValid()
        {
            if (N < 1 || N > 500) return false;
            if (M < 1 || M > 500) return false;
            if (P < 1 || P > N * M) return false;

            // Check matrix dimensions
            if (Matrix == null || Matrix.Count != N * M)
                return false;

            // Check if all numbers in matrix are valid (between 1 and P)
            foreach (var kvp in Matrix)
            {
                if (kvp.Value < 1 || kvp.Value > P)
                    return false;
            }

            // Check if there's exactly one chest numbered P
            int countP = 0;
            foreach (var kvp in Matrix)
            {
                if (kvp.Value == P)
                    countP++;
            }
            if (countP != 1) return false;

            return true;
        }
    }

    public static class MatrixConverter
    {
        public static int[,] ToDimensionalArray(this Dictionary<string, int> dict, int n, int m)
        {
            var result = new int[n, m];
            foreach (var kvp in dict)
            {
                var coords = kvp.Key.Split(',');
                if (coords.Length == 2 && 
                    int.TryParse(coords[0], out int i) && 
                    int.TryParse(coords[1], out int j))
                {
                    if (i >= 0 && i < n && j >= 0 && j < m)
                    {
                        result[i, j] = kvp.Value;
                    }
                }
            }
            return result;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TreasureHunt.Core.Entities;
using TreasureHunt.Core.Interfaces;
using TreasureHunt.Core.Models;
using TreasureHunt.Infrastructure.Data;

namespace TreasureHunt.Application.Services
{
    public class TreasureHuntService : ITreasureHuntService
    {
        private readonly ApplicationDbContext _context;

        public TreasureHuntService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<double> SolveTreasureHuntAsync(TreasureMapRequest request)
        {
            if (!request.IsValid())
                throw new ArgumentException("Invalid request data.");

            var matrix = request.Matrix.ToDimensionalArray(request.N, request.M);
            var result = FindMinimumFuel(request.N, request.M, request.P, matrix);

            var treasureMap = new TreasureMap
            {
                N = request.N,
                M = request.M,
                P = request.P,
                MatrixData = JsonSerializer.Serialize(request.Matrix),
                Result = result,
                CreatedAt = DateTime.UtcNow
            };

            _context.TreasureMaps.Add(treasureMap);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<TreasureMap>> GetHistoryAsync()
        {
            return await _context.TreasureMaps
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();
        }

        /// <summary>
        /// Tính lượng nhiên liệu tối thiểu để mở kho báu, theo thứ tự mở rương từ 1 đến p.
        /// Hải tặc bắt đầu từ điểm (0,0) tương ứng với hàng 1, cột 1 theo đề bài.
        /// </summary>
        /// <param name="n">Số hàng của ma trận</param>
        /// <param name="m">Số cột của ma trận</param>
        /// <param name="p">Số thứ tự rương chứa kho báu (từ 1 đến p)</param>
        /// <param name="matrix">Ma trận các hòn đảo, mỗi vị trí chứa số rương (1 <= a[i][j] <= p)</param>
        /// <returns>Chi phí nhiên liệu tối thiểu để lấy được kho báu</returns>
        private double FindMinimumFuel(int n, int m, int p, int[,] matrix)
        {
            // 1. Thu thập vị trí các rương: key = số rương, value = danh sách tọa độ (i, j)
            Dictionary<int, List<(int Row, int Col)>> chestPositions = CollectChestPositions(n, m, matrix);

            // Kiểm tra xem ma trận có đủ các chest từ 1 đến p không
            for (int chestNumber = 1; chestNumber <= p; chestNumber++)
            {
                if (!chestPositions.ContainsKey(chestNumber))
                    throw new Exception($"Không tìm thấy chest {chestNumber} trong ma trận!");
            }

            // 2. Sử dụng DP để tính khoảng cách tối thiểu từ điểm xuất phát đến chest p theo thứ tự.
            // dp: lưu trạng thái hiện tại cho chest số (k-1), dưới dạng map tọa độ -> chi phí tối thiểu.
            Dictionary<(int Row, int Col), double> dp = new Dictionary<(int, int), double>();

            // Bắt đầu từ điểm xuất phát (0,0): tính khoảng cách đến các chest số 1
            foreach (var pos in chestPositions[1])
            {
                double cost = EuclideanDistance((0, 0), pos);
                dp[pos] = cost;
            }

            // Tính dp cho các chest từ 2 đến p
            for (int chest = 2; chest <= p; chest++)
            {
                dp = UpdateDP(dp, chestPositions[chest]);
            }

            // Kết quả là giá trị nhỏ nhất ở dp của chest p
            return dp.Values.Min();
        }

        /// <summary>
        /// Duyệt ma trận và thu thập vị trí của từng chest theo số
        /// </summary>
        private Dictionary<int, List<(int Row, int Col)>> CollectChestPositions(int n, int m, int[,] matrix)
        {
            var positions = new Dictionary<int, List<(int, int)>>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int chestNumber = matrix[i, j];
                    if (!positions.ContainsKey(chestNumber))
                    {
                        positions[chestNumber] = new List<(int, int)>();
                    }
                    positions[chestNumber].Add((i, j));
                }
            }
            return positions;
        }

        /// <summary>
        /// Cập nhật dp: từ các trạng thái của chest trước đó (prevStates) đến tất cả các vị trí chứa chest hiện tại.
        /// Trả về map mới: tọa độ -> chi phí tối thiểu để đến đó.
        /// </summary>
        private Dictionary<(int Row, int Col), double> UpdateDP(
            Dictionary<(int, int), double> prevStates,
            List<(int Row, int Col)> currentChestPositions)
        {
            var newDP = new Dictionary<(int, int), double>();

            // Với mỗi vị trí chứa chest hiện tại, tìm đường đi tối ưu từ mọi vị trí của chest trước đó.
            foreach (var currentPos in currentChestPositions)
            {
                double bestCost = double.MaxValue;
                foreach (var prevEntry in prevStates)
                {
                    double distance = EuclideanDistance(prevEntry.Key, currentPos);
                    double candidateCost = prevEntry.Value + distance;
                    if (candidateCost < bestCost)
                        bestCost = candidateCost;
                }
                newDP[currentPos] = bestCost;
            }
            return newDP;
        }

        /// <summary>
        /// Tính khoảng cách Euclid giữa hai tọa độ
        /// </summary>
        private double EuclideanDistance((int Row, int Col) pos1, (int Row, int Col) pos2)
        {
            int dRow = pos1.Row - pos2.Row;
            int dCol = pos1.Col - pos2.Col;
            return Math.Sqrt(dRow * dRow + dCol * dCol);
        }
    }
}
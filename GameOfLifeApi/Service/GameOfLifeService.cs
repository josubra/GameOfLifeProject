using GameOfLifeApi.Data.Repository.Abstraction;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GameOfLifeApi.Service
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IRepository<GameOfLifeBoard> _repository;
        public GameOfLifeService(IRepository<GameOfLifeBoard> repository) 
        {
            _repository = repository;
        }

        public async Task<GameOfLifeBoard> AddGameOfLifeBoardAsync(bool[][] boardState, CancellationToken cancellationToken = default)
        {
            GameOfLifeBoard board = new GameOfLifeBoard
            {
                LastBoardState = JsonSerializer.Serialize(boardState)
            };
            return await _repository.AddAsync(board, cancellationToken);
        }

        public async Task<GameOfLifeBoard> GetGameOfLifeBoardAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<bool[][]?> GetAndSaveNextStateAsync(int id)
        {
            var (board, state) = await GetBoardStateAsync(id);
            if (board == null || state == null) return null;
            var nextState = ComputeNextState(state);
            board.LastBoardState = JsonSerializer.Serialize(nextState);
            await _repository.SaveAsync();
            return nextState;

        }

        public async Task<bool[][]?> GetAndSaveFutureStateAsync(int id, int steps)
        {
            var (board, state) = await GetBoardStateAsync(id);
            if (board == null || state == null) return null;
            for (int i = 0; i < steps; i++)
            {
                state = ComputeNextState(state);
            }
            board.LastBoardState = JsonSerializer.Serialize(state);
            await _repository.SaveAsync();
            return state;
        }

        public async Task<bool[][]?> GetAndSaveFinalStateAsync(int id, int maxAttempts)
        {
            var (board, state) = await GetBoardStateAsync(id);
            if (board == null || state == null) return null;
            for (int i = 0; i < maxAttempts; i++)
            {
                var nextState = ComputeNextState(state);
                if (AreBoardsEqual(nextState, state))
                {
                    board.LastBoardState = JsonSerializer.Serialize(nextState);
                    await _repository.SaveAsync();
                    return state;
                }
                state = nextState;
            }
            return null;
        }

        private bool[][] ComputeNextState(bool[][] board)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            bool[][] newBoard = new bool[rows][];
            for (int r = 0; r < rows; r++)
            {
                newBoard[r] = new bool[cols];
                for (int c = 0; c < cols; c++)
                {
                    int liveNeighbors = CountLiveNeighbors(board, r, c);
                    if (board[r][c])
                    {
                        newBoard[r][c] = liveNeighbors == 2 || liveNeighbors == 3;
                    }
                    else
                    {
                        newBoard[r][c] = liveNeighbors == 3;
                    }
                }
            }
            return newBoard;
        }

        private int CountLiveNeighbors(bool[][] board, int row, int col)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            int count = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    int newRow = row + dr;
                    int newCol = col + dc;
                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow][newCol])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool AreBoardsEqual(bool[][] board1, bool[][] board2)
        {
            int rows = board1.Length;
            int cols = board1[0].Length;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (board1[r][c] != board2[r][c]) return false;
                }
            }
            return true;
        }

        private async Task<(GameOfLifeBoard?, bool[][]?)> GetBoardStateAsync(int id)
        {
            var board = await _repository.GetByIdAsync(id);
            return board == null ? (null, null) : (board, board.DeserializeBoard());
        }
    }
}

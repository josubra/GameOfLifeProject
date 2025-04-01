using GameOfLifeApi.Entity;
using System.Text.Json;

namespace GameOfLifeApi.Util
{
    public static class HelperUtil
    {
        public static bool[][] GenerateBoard(int rows, int columns)
        {
            var board = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                board[i] = new bool[columns];
                for (int j = 0; j < columns; j++)
                {
                    board[i][j] = new Random().Next(2) == 0;
                }
            }
            return board;
        }

        public static bool ValidateBoard(bool[][] jaggedArray)
        {
            if (jaggedArray == null || jaggedArray.Length < 3)
                return false;

            int expectedLength = jaggedArray[0].Length;

            for(int i = 1; i < jaggedArray.Length; i++)
            {
                if (jaggedArray[i] == null || jaggedArray[i].Length != expectedLength)
                    return false;
            }

            return true;
        }

        public static bool[][] SampleBoard => new bool[][]
        {
            new bool[] { false, true, false },
            new bool[] { false, true, false },
            new bool[] { false, true, false }
        };

        public static bool[][] EmptyBoard => new bool[][]
        {
            new bool[] { false, false, false },
            new bool[] { false, false, false },
            new bool[] { false, false, false }
        };

        public static bool[][] BlockBoard = new bool[][]
        {
            new bool[] { false, false, false, false },
            new bool[] { false, true,  true,  false },
            new bool[] { false, true,  true,  false },
            new bool[] { false, false, false, false }
        };

        public static bool[][] BlinkerBoard1 = new bool[][]
        {
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, true,  false, false },
            new bool[] { false, false, true,  false, false },
            new bool[] { false, false, true,  false, false },
            new bool[] { false, false, false, false, false }
        };

        public static bool[][] BlinkerBoard2 = new bool[][]
        {
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, true,  true,  true,  false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false }
        };

        public static bool[][] SingleCellBoard = new bool[][]
        {
            new bool[] { false, false, false },
            new bool[] { false, true,  false },
            new bool[] { false, false, false }
        };

        public static bool[][] InvalidBoard => new bool[][]
        {
            new bool[] { false, true },
            new bool[] { false },
            new bool[] { false, true, false }
        };

        public static GameOfLifeBoard CreateTestBoard(int id, bool[][] boardState)
        {
            return new GameOfLifeBoard
            {
                Id = id,
                LastBoardState = JsonSerializer.Serialize(boardState)
            };
        }

        public static bool[][] ComputeNextState(bool[][] board)
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

        public static int CountLiveNeighbors(bool[][] board, int row, int col)
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

        public static bool AreBoardsEqual(bool[][] board1, bool[][] board2)
        {
            int rowsBoard1 = board1.Length;
            int colsBoard1 = board1[0].Length;
            int rowsBoard2 = board2.Length;
            int colsBoard2 = board2[0].Length;

            if (rowsBoard1 != rowsBoard2 || colsBoard1 != colsBoard2) return false;

            for (int r = 0; r < rowsBoard1; r++)
            {
                for (int c = 0; c < colsBoard1; c++)
                {
                    if (board1[r][c] != board2[r][c]) return false;
                }
            }
            return true;
        }
    }
}

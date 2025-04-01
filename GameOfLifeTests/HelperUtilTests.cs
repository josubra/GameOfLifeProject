using DotNet.Testcontainers;
using GameOfLifeApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeTests
{
    public class HelperUtilTests
    {
        [Fact]
        public void CountLiveNeighbors_EmptyBoard_ReturnsZero()
        {
            var board = HelperUtil.EmptyBoard;

            for (int r = 0; r < board.Length; r++)
            {
                for (int c = 0; c < board[0].Length; c++)
                {
                    var count = HelperUtil.CountLiveNeighbors(board, r, c);
                    Assert.Equal(0, count);
                }
            }
        }

        [Fact]
        public void CountLiveNeighbors_CenterOfBlock_ReturnsThree()
        {
            var board = HelperUtil.BlockBoard;
            int centerRow = 1;
            int centerColumn = 1;

            var count = HelperUtil.CountLiveNeighbors(board, centerRow, centerColumn);

            Assert.Equal(3, count);
        }

        [Fact]
        public void CountLiveNeighbors_CornerCell_ReturnsCorrectCount()
        {
            var board = HelperUtil.BlockBoard;
            int cornerRow = 0;
            int cornerColumn = 0;

            var count = HelperUtil.CountLiveNeighbors(board, cornerRow, cornerColumn);

            Assert.Equal(1, count);
        }

        [Fact]
        public void ComputeNextState_EmptyBoard_RemainsEmpty()
        {
            var board = HelperUtil.EmptyBoard;

            var nextState = HelperUtil.ComputeNextState(board);

            Assert.True(HelperUtil.AreBoardsEqual(board, nextState));
        }

        [Fact]
        public void ComputeNextState_BlockBoard_RemainsUnchanged()
        {
            var board = HelperUtil.EmptyBoard;

            var nextState = HelperUtil.ComputeNextState(board);

            Assert.True(HelperUtil.AreBoardsEqual(board, nextState));
        }

        [Fact]
        public void ComputeNextState_Blinker_OscillatesCorrectly()
        {
            var phase1 = HelperUtil.BlinkerBoard1;
            var phase2 = HelperUtil.BlinkerBoard2;

            var computedPhase2 = HelperUtil.ComputeNextState(phase1);
            var computedPhase1 = HelperUtil.ComputeNextState(phase2);

            Assert.True(HelperUtil.AreBoardsEqual(phase2, computedPhase2));
            Assert.True(HelperUtil.AreBoardsEqual(phase1, computedPhase1));
        }

        [Fact]
        public void ComputeNextState_SingleCell_Dies()
        {
            var board = HelperUtil.SingleCellBoard;

            var nextState = HelperUtil.ComputeNextState(board);

            Assert.True(HelperUtil.AreBoardsEqual(HelperUtil.EmptyBoard, nextState));
        }

        [Fact]
        public void CheckInvalidBoard_ShouldReturnFalse()
        {
            var board = HelperUtil.InvalidBoard;

            var isValid = HelperUtil.ValidateBoard(board);

            Assert.False(isValid);
        }

        [Fact]
        public void CheckValidBoard_ShouldReturnTrue()
        {
            var board = HelperUtil.BlockBoard;

            var isValid = HelperUtil.ValidateBoard(board);

            Assert.True(isValid);
        }

        [Fact]
        public void AreBoardsEqual_IdenticalBoards_ReturnsTrue()
        {
            var board1 = HelperUtil.BlockBoard;
            var board2 = HelperUtil.BlockBoard.Select(row => row.ToArray()).ToArray();

            var result = HelperUtil.AreBoardsEqual(board1, board2);

            Assert.True(result);
        }

        [Fact]
        public void AreBoardsEqual_DifferentBoards_ReturnsFalse()
        {
            var board1 = HelperUtil.BlockBoard;
            var board2 = HelperUtil.SingleCellBoard;

            var result = HelperUtil.AreBoardsEqual(board1, board2);

            Assert.False(result);
        }

        [Fact]
        public void AreBoardsEqual_SingleDifference_ReturnsFalse()
        {
            var board1 = HelperUtil.BlockBoard;
            var board2 = HelperUtil.BlockBoard.Select(row => row.ToArray()).ToArray();
            board2[1][1] = !board2[1][1];

            var result = HelperUtil.AreBoardsEqual(board1, board2);

            Assert.False(result);
        }

        [Fact]
        public void CountLiveNeighbors_AllLive_Returns8ForCenter()
        {
            var board = new bool[][]
            {
                new bool[] { true, true, true },
                new bool[] { true, true, true },
                new bool[] { true, true, true }
            };

            var count = HelperUtil.CountLiveNeighbors(board, 1, 1);

            Assert.Equal(8, count);
        }

        [Fact]
        public void ComputeNextState_Overpopulation_Dies()
        {
            var board = new bool[][]
            {
                new bool[] { true, true, true },
                new bool[] { true, true, true },
                new bool[] { false, false, false }
            };

            var nextState = HelperUtil.ComputeNextState(board);

            Assert.False(nextState[1][1]); // Center cell dies from overpopulation
        }
    }
}

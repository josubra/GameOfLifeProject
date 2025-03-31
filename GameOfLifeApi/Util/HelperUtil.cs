namespace GameOfLifeApi.Util
{
    public static class HelperUtil
    {
        public static bool[][] GenerateFirstBoard(int rows, int columns)
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

        public static bool ValidateJaggedArray(bool[][] jaggedArray)
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
    }
}

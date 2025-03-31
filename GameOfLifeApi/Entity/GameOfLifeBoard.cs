namespace GameOfLifeApi.Entity
{
    public class GameOfLifeBoard 
    {
        public int Id { get; set; }

        public string LastBoardState { get; set; } = string.Empty;

        public bool[][] DeserializeBoard()
        {
            return System.Text.Json.JsonSerializer.Deserialize<bool[][]>(this.LastBoardState) ?? new bool[0][];
        }
    }
}

namespace GameOfLifeApi.Entity
{
    public class GameOfLifeBoard 
    {
        public int Id { get; set; }

        public string LastBoardState { get; set; } = string.Empty;
    }
}

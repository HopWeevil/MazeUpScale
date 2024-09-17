namespace CodeBase.Logic.Maze
{
    public class CellToVisit
    {
        public int Row { get; }
        public int Column { get; }
        public Direction MoveMade { get; }

        public CellToVisit(int row, int column, Direction moveMade)
        {
            Row = row;
            Column = column;
            MoveMade = moveMade;
        }
    }
}
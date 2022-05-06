namespace Project.Bot
{
    public class CellData
    {
        public float HCost;
        public float GCost;

        public float FCost => HCost + GCost;

        public Cell Cell;
        public int Id;
        public CellData Parent;
    }
}
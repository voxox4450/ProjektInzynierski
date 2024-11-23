namespace Harmonogram.Common.Entities
{
    public class Day
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<WorkBlock> WorkBlocks { get; set; } = new List<WorkBlock>();
    }
}
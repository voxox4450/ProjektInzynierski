namespace Harmonogram.Common.Entities
{
    public class Day
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<WorkBlock> WorkBlocks { get; set; } = [];
    }
}
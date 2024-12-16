namespace Harmonogram.Common.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hex { get; set; } = string.Empty;

        public ICollection<WorkBlock> WorkBlocks { get; set; } = new List<WorkBlock>();
    }
}
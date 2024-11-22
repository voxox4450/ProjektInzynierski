namespace Harmonogram.Common.Interfaces
{
    public interface IConstants
    {
        IEnumerable<string> SetHours();

        DateTime SetStartDate();

        DateTime SetEndDate();

        string SetBaseName(DateTime startDate);
    }
}
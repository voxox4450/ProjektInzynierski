using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
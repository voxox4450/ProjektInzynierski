using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Common.Interfaces
{
    public interface IDayRepository
    {
        int GetDayId(string dayName);
    }
}
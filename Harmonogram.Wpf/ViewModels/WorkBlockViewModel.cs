using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Harmonogram.Wpf.ViewModels
{
    public class WorkBlockViewModel
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Brush Fill { get; set; }
        public double Opacity { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public int Row { get; set; }
    }
}

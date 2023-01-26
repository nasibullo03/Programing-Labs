using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    interface IOutputValue
    {
        List<Point> FunctionCoordinates { get; }
        List<Point> SplitCoordinates { get;}
        double? OptimalSplitValue { get; }
    }
}

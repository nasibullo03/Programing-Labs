using System;
using System.Collections.Generic;
using System.Windows;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    public interface IEnteredValues
    {
        double A { get; set; }
        double B { get; set; }
        double N { get; set; }
        double E { get; set; }
        Func<double, double> F { get; set; }
        Func<IEnteredValues, List<Point>> GetFunctionCoordinates { get; set; }
        void SetValues(IEnteredValues values);


    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    public class RectangleMethod : IEnteredValues, IOutputValue
    {
        #region prorerties        
        #region IEnteredValue
        public double A { get; set; }
        public double B { get; set; }
        public double N { get; set; }
        public double E { get; set; }
        public Func<double, double> F { get; set; }
        public Func<IEnteredValues, List<Point>> GetFunctionCoordinates { get; set; }
        #endregion

        #region IOutputValue    
        public List<Point> FunctionCoordinates => GetFunctionCoordinates(this);
        public List<Point> SplitCoordinates { get; private set; }
       
        public double? OptimalSplitValue { get; set; }
       


        #endregion

        #region OtherProperties
        private List<Point> LeftSplitCoordinates { get; set; }
        private List<Point> RightSplitCoordinates { get; set; }
        private List<Point> MiddleSplitCoordinates { get; set; }
        public enum RectangleType { Left, Right, Central }
        public RectangleType MethodType { get; set; }



        #endregion

        #endregion

        #region Constructors
        public RectangleMethod(RectangleType methodType)
        {
            MethodType = methodType;
        }
        public RectangleMethod(IEnteredValues values, RectangleType methodType)
        {
            A = values.A;
            B = values.B;
            E = values.E;
            N = values.E;
            F = values.F;
            MethodType = methodType;

            switch (MethodType)
            {
                case RectangleType.Left:
                    OptimalSplitValue = LeftRectangle();
                    SplitCoordinates = LeftSplitCoordinates;
                    break;
                case RectangleType.Right:
                    OptimalSplitValue = RightRectangle();
                    SplitCoordinates = RightSplitCoordinates;
                    break;
                case RectangleType.Central:
                    OptimalSplitValue = CentralRectangle();
                    SplitCoordinates = MiddleSplitCoordinates;
                    break;
               
            }
            
        }
    
        
        #endregion

        #region Interface Methods
        public void SetValues(IEnteredValues values)
        {
            A = values.A;
            B = values.B;
            E = values.E;
            N = values.N;
            F = values.F;
            GetFunctionCoordinates = values.GetFunctionCoordinates;

            switch (MethodType)
            {
                case RectangleType.Left:
                    OptimalSplitValue = LeftRectangle();
                    SplitCoordinates = LeftSplitCoordinates;
                    break;
                case RectangleType.Right:
                    OptimalSplitValue = RightRectangle();
                    SplitCoordinates = RightSplitCoordinates;
                    break;
                case RectangleType.Central:
                    OptimalSplitValue = CentralRectangle();
                    SplitCoordinates = MiddleSplitCoordinates;
                    break;

            }

        }

        #endregion

        #region RectangleResultMethods
        private double LeftRectangle()
        {
            LeftSplitCoordinates = new List<Point>();
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;
            
            for (int i = 0; i <= N - 1; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
                LeftSplitCoordinates.Add(new Point(x, y));
            }

            LeftSplitCoordinates.Add(new Point(B, F(B)));
            double result = h * sum;
            return result;
        }
        private double RightRectangle()
        {
            RightSplitCoordinates = new List<Point>();
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;
           
            for (var i = 1; i <= N; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
                RightSplitCoordinates.Add(new Point(x, y));
            }
            RightSplitCoordinates.Add(new Point(B, F(B)));
            double result = h * sum;
            return result;
        }
        private double CentralRectangle()
        {
            MiddleSplitCoordinates = new List<Point>();
            double h = (B - A) / N;
            double sum = (F(A) + F(B)) / 2;
            double x = 0d;
            double y = 0d;
            
            for (var i = 1; i < N; i++)
            {
                x = A + h * i;
                y = F(x);
                sum += y;
                MiddleSplitCoordinates.Add(new Point(x, y));
            }
            MiddleSplitCoordinates.Add(new Point(B, F(B)));
            double result = h * sum;
            return result;
        }

        #endregion

    }
}
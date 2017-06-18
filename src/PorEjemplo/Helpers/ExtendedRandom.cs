using System;
using System.Text;

namespace PorEjemplo.Helpers {

    public static class ExtendedRandom {

        private static Random Random { get; } = new Random();

        public static string NextString(int length) {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++) {
                sb.Append((char)Random.Next(97, 123));
            }
            return sb.ToString();
        }

        public static string NextString(int minLength, int maxLength) {
            var length = Random.Next(minLength, maxLength + 1);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append((char)Random.Next(97, 123));
            }
            return sb.ToString();
        }


        public static double NextDouble(double minValue, double maxValue) {
            if (minValue > maxValue) throw new ArgumentException("The min value must not be bigger than the max value");
            double number = Random.Next((int)Math.Ceiling(minValue), (int)Math.Floor(maxValue));

            if(Random.Next(0, 2) == 1) {
                number += Random.NextDouble();
                number = number > maxValue ? maxValue : number;
            } else {
                number -= Random.NextDouble();
                number = number < minValue ? maxValue : number;
            }

            return number;
        }

        public static bool NextBoolean() {
            return Random.Next(0, 2) == 0;
        }

        public static char NextChar() {
            return (char)Random.Next(97, 123);
        }

        public static DateTime NextDateTime() {
            var year = Random.Next(1000, 3000);
            var month = Random.Next(1, 13);
            var day = Random.Next(1, DateTime.DaysInMonth(year, month) + 1);
            return new DateTime(year, month, day);
        }
    }
}
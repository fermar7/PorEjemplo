using System;
using System.Collections.Generic;

namespace PorEjemplo.Extensions {
    public static class Extensions {

        public static void Print<T>(this IEnumerable<T> collection) {
            foreach (var item in collection) {
                Console.WriteLine(item);
            }
        }

    }
}
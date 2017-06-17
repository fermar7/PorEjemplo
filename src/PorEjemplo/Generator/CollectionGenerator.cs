using PorEjemplo.Helpers;
using System;
using System.Collections.Generic;

namespace PorEjemplo.Generator {

    public sealed class CollectionGenerator<TSource> : IGenerator where TSource : new() {

        private static Random Random { get; } = new Random();

        private int Length { get; }
        private Func<TSource> GeneratorFunc { get; set; }
        private IGenerator GeneratorObject { get; set; }

        public CollectionGenerator(Func<TSource> generator) : this(Random.Next(20), generator) { }

        public CollectionGenerator(int length, Func<TSource> generator) {
            Length = Random.Next(20);
            GeneratorFunc = generator;
        }

        public CollectionGenerator(IGenerator generator) : this(Random.Next(20), generator) { }

        public CollectionGenerator(int length, IGenerator generator) {
            Length = Random.Next(20);
            GeneratorObject = generator;
        }

        public object Generate() {
            IList<TSource> list = new List<TSource>(Length);

            try {

                for (int i = 0; i < Length; i++) {
                    if (GeneratorFunc != null) {
                        list.Add(GeneratorFunc());
                    } else {
                        list.Add((TSource)GeneratorObject.Generate());
                    }
                }

            } catch (InvalidCastException) {
                throw new ArgumentException("Generator type not matching collection type");
            }

            return list;
        }

    }
}

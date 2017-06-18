using PorEjemplo.Generator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PorEjemplo.Test.GeneratorTests {

    class CollectionGeneratorTests {

        [Fact]
        public void Generate() {
            Random random = new Random();
            CollectionGenerator<int> generator = new CollectionGenerator<int>(10, random.Next);

            Assert.NotNull(generator);

            var generated = (List<int>) generator.Generate();

            Assert.Equal(10, generated.Count);
        }

        [Fact]
        public void Generate_SubGenerator() {
            Random random = new Random();
            CollectionGenerator<int> generator = new CollectionGenerator<int>(10, new IntGenerator());

            Assert.NotNull(generator);

            var generated = (List<int>)generator.Generate();

            Assert.Equal(10, generated.Count);
            Assert.True(generated.TrueForAll(_ => _ == -5));
        }

        private class IntGenerator : IGenerator {

            public object Generate() {
                return -5;
            }
        }

    }
}

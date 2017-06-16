using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PorEjemplo.Generator;
using Xunit;

namespace PorEjemplo.Test {

    public class GeneratorTests {

        [Fact]
        public void SetupGenerator() {

            var generator = new Generator<Test>();
            Assert.NotNull(generator);

            var generated = (Test) generator.Generate();
            Assert.NotNull(generated.Text);
            Assert.True(generated.Number != 0);
        }

        [Fact]
        public void Member_Ignore() {
            var generator = Generator<Test>.Setup()
                                .ForMember(_ => _.Text).Ignore()
                                .GetGenerator();
            Assert.NotNull(generator);
            var generated = (Test)generator.Generate();

            Assert.Null(generated.Text);
            Assert.True(generated.Number != 0);
        }

        [Fact]
        public void Member_UseValue() {
            var text = "HardCoded";

            var generator = Generator<Test>.Setup()
                                .ForMember(_ => _.Text).UseValue(text)
                                .GetGenerator();
            Assert.NotNull(generator);
            var generated = (Test)generator.Generate();

            Assert.Equal(text, generated.Text);
            Assert.True(generated.Number != 0);
        }

        [Fact]
        public void Member_UseGenerator() {

            var generator = Generator<Test>.Setup()
                                .ForMember(_ => _.Number).UseGenerator(new IntGenerator())
                                .GetGenerator();
            Assert.NotNull(generator);
            var generated = (Test)generator.Generate();

            Assert.Equal(-5, generated.Number);
            Assert.True(generated.Number != 0);
        }

        [Fact]
        public void Member_UseGenerator_WrongType() {

            var generator = Generator<Test>.Setup()
                                .ForMember(_ => _.Text).UseGenerator(new IntGenerator())
                                .GetGenerator();
            Assert.NotNull(generator);

            Assert.Throws<ArgumentException>(() => generator.Generate());
        }

        [Fact]
        public void Type_UseGenerator() {

            var generator = Generator<Test>.Setup()
                                .ForType<int>().UseGenerator(new IntGenerator())
                                .GetGenerator();
            Assert.NotNull(generator);
            var generated = (Test)generator.Generate();

            Assert.Equal(-5, generated.Number);
            Assert.True(generated.Number != 0);
        }

        [Fact]
        public void Type_UseGenerator_WrongType() {

            var generator = Generator<Test>.Setup()
                                .ForType<string>().UseGenerator(new IntGenerator())
                                .GetGenerator();
            Assert.NotNull(generator);

            Assert.Throws<ArgumentException>(() => generator.Generate());
        }

        private class IntGenerator : IGenerator {

            public object Generate() {
                return -5;
            }
        }

        private class Test {

            public string Text { get; set; }

            public int Number { get; set; }

        }

    }
}

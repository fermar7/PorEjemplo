using System;
using System.Collections.Generic;
using System.Text;
using PorEjemplo.Generator;
using Xunit;

namespace PorEjemplo.Test {

    public class GeneratorTests {

        [Fact]
        public void SetupGen() {

            var gen = Generator<Test>.Setup().ForMember(_ => _.Text).UseValue("Hallo");

        }

        public class Test {
            public string Text { get; set; }
        }

    }
}

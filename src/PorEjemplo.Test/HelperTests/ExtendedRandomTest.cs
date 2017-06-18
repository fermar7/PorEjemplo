using PorEjemplo.Helpers;
using Xunit;

namespace PorEjemplo.Test.HelperTests {

    public class ExtendedRandomTest {

        [Fact]
        public void GenRandomString() {
            var length = 5;

            var newString = ExtendedRandom.NextString(length);

            Assert.Equal(length, newString.Length);

        }

        [Fact]
        public void GenRandomStringMinMaxLength() {
            var min = 1;
            var max = 10;

            var newString = ExtendedRandom.NextString(min, max);
            Assert.True(newString.Length >= min && newString.Length <= max);
        }


        [Fact]
        public void GenRandomDouble() {
            var min = 1.2;
            var max = 2.5;
            
            for (int i = 0; i < 20; i++) {
                var newDouble = ExtendedRandom.NextDouble(min, max);
                Assert.True(newDouble >= min && newDouble <= max);
            }
        }

        [Fact]
        public void GenRandomDate() {
            var dateTime = ExtendedRandom.NextDateTime();
        }

    }
}

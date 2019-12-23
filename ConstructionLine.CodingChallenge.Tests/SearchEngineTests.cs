using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        [TestCase("Red")]
        [TestCase("Blue")]
        [TestCase("Yellow")]
        [TestCase("White")]
        [TestCase("Black")]
        public void Given_SomeShirts_When_QueriedWithAnExistingColor_Then_Returns_ShirtsOfThatColor(string color)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions() { Colors = Color.All.Where(x => x.Name == color).ToList() };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        [TestCase("Small")]
        [TestCase("Medium")]
        [TestCase("Large")]
        public void Given_SomeShirts_When_QueriedWithAnExistingSize_Then_Returns_ShirtsOfThatSize(string size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions() { Sizes = Size.All.Where(x => x.Name == size)
                .ToList() };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [TestCase("Red", "Small")]
        [TestCase("Red", "Medium")]
        [TestCase("Red", "Large")]
        [TestCase("Blue", "Small")]
        [TestCase("Blue", "Medium")]
        [TestCase("Blue", "Large")]
        [TestCase("Yellow", "Small")]
        [TestCase("Yellow", "Medium")]
        [TestCase("Yellow", "Large")]
        [TestCase("Black", "Small")]
        [TestCase("Black", "Medium")]
        [TestCase("Black", "Large")]
        [TestCase("White", "Small")]
        [TestCase("White", "Medium")]
        [TestCase("White", "Large")]
        public void Given_SomeShirts_When_QueriedWithColorAndASize_Then_Returns_ShirtsOfRedColorAndThatSize(string color, string size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = Size.All.Where(x => x.Name == size).ToList(),
                Colors = Color.All.Where(x => x.Name == color).ToList()
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        [TestCase("Red", new string[] { "Small", "Large", "Medium" })]
        [TestCase("Blue", new string[] { "Small", "Large", "Medium" })]
        [TestCase("Black", new string[] { "Small", "Large", "Medium" })]
        [TestCase("Yellow", new string[] { "Small", "Large", "Medium" })]
        [TestCase("White", new string[] { "Small", "Large", "Medium" })]
        public void Given_SomeShirts_When_QueriedWithSingleColorAndMultipleSizes_Then_Returns_ShirtsOfColorAndThoseSizes(string color, string[] size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = Size.All.Where(x => size.Contains(x.Name)).ToList(),
                Colors = Color.All.Where(x => x.Name == color).ToList()
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        [TestCase(new string[] { "Red", "Yellow", "White" }, "Small")]
        [TestCase(new string[] { "Red", "Yellow", "White", "Black" }, "Large")]
        public void Given_SomeShirts_When_QueriedWithMultipleColorAndSingleSize_Then_Returns_ShirtsOfColorsAndThoseSize(string[] color, string size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = Size.All.Where(x => x.Name == size).ToList(),
                Colors = Color.All.Where(x => color.Contains(x.Name)).ToList()
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        [TestCase(new string[] { "Red", "Yellow", "White" }, new string[] { "Small", "Large" })]
        [TestCase(new string[] { "Red", "Yellow", "White", "Black" }, new string[] {"Medium", "Small"})]
        public void Given_SomeShirts_When_QueriedWithMultipleColorAndMultipleSizes_Then_Returns_ShirtsOfColorsAndThoseSizes(string[] color, string[] size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = Size.All.Where(x => size.Contains(x.Name)).ToList(),
                Colors = Color.All.Where(x => color.Contains(x.Name)).ToList()
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        [TestCase(new string[] { "Red", "Yellow", "White" }, new string[] { "Small", "Large" })]
        [TestCase(new string[] { "Red", "Yellow", "White", "Black" }, new string[] { "Medium", "Small" })]
        public void Given_NoShirts_When_QueriedWithMultipleColorAndMultipleSizes_Then_Returns_EmptyResults(string[] color, string[] size)
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(0).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = Size.All.Where(x => size.Contains(x.Name)).ToList(),
                Colors = Color.All.Where(x => color.Contains(x.Name)).ToList()
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(searchResults.Shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(searchResults.Shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        public void Given_SomeShirts_When_QueriedWithNoOptions_Then_Returns_EmptyResults()
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
              
            };

            // Act
            var searchResults = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(searchResults.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, searchResults.SizeCounts);
            AssertColorCounts(shirts, searchOptions, searchResults.ColorCounts);
        }

        [Test]
        public void Given_SomeShirts_When_QueriedWithNullColorOption_Then_Throws_ArgumentNullException()
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Colors = null
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => { searchEngine.Search(searchOptions); });
        }

        [Test]
        public void Given_SomeShirts_When_QueriedWithNullSizeOption_Then_Throws_ArgumentNullException()
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions()
            {
                Sizes = null
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => { searchEngine.Search(searchOptions); });
        }

        [Test]
        public void Given_SomeShirts_When_QueriedWithNullOption_Then_Throws_ArgumentNullException()
        {
            // Arrange
            var shirts = new SampleData.SampleDataBuilder(50).CreateShirts();
            var searchEngine = new SearchEngine(shirts);
           
            // Assert
            Assert.Throws<ArgumentNullException>(() => { searchEngine.Search(null); });
        }

    }
}

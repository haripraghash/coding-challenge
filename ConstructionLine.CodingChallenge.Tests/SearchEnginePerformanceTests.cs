using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEnginePerformanceTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;
        private SearchEngine _searchEngine;

        [SetUp]
        public void Setup()
        {

            var dataBuilder = new SampleDataBuilder(50000);

            _shirts = dataBuilder.CreateShirts();

            _searchEngine = new SearchEngine(_shirts);
        }


        [Test]
        public void PerformanceTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = new List<Color> { Color.Red }
            };

            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(results.Shirts, options, results.SizeCounts);
            AssertColorCounts(results.Shirts, options, results.ColorCounts);
        }

        [Test]
        public void PerformanceTest_WithMultiple_Options()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = new List<Color> { Color.Red, Color.Black },
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(results.Shirts, options, results.SizeCounts);
            AssertColorCounts(results.Shirts, options, results.ColorCounts);
        }

        [Test]
        public void PerformanceTest_WithAll_Options()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = Color.All,
                Sizes = Size.All
            };

            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(results.Shirts, options, results.SizeCounts);
            AssertColorCounts(results.Shirts, options, results.ColorCounts);
        }
    }
}

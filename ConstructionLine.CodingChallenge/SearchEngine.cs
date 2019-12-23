using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private Dictionary<Color, List<Shirt>> _shirtsIndexedByColor;
        private Dictionary<Size, List<Shirt>> _shirtsIndexedBySize;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // Optimizing for look up performance
            _shirtsIndexedByColor = shirts.GroupBy(x => x.Color).ToDictionary(y => y.Key, z => z.ToList());
            _shirtsIndexedBySize = shirts.GroupBy(x => x.Size).ToDictionary(y => y.Key, z => z.ToList());
        }


        public SearchResults Search(SearchOptions options)
        {
            IEnumerable<Shirt> results = Enumerable.Empty<Shirt>();

            if (options == null || options.Colors == null || options.Sizes == null)
            {
                throw new ArgumentNullException("options");
            }

            if (options.Colors.Any())
            {
                results = _shirtsIndexedByColor.Where(x => options.Colors.Contains(x.Key)).SelectMany(y => y.Value);
            }

            if (options.Sizes.Any())
            {
                results = results.Any() ? results.Where(x => options.Sizes.Contains(x.Size)) : _shirtsIndexedBySize.Where(x => options.Sizes.Contains(x.Key)).SelectMany(x => x.Value);
            }

            var finalResults = !results.Any() ? _shirts.ToList() : results.ToList();
            return new SearchResults
            {
                Shirts = finalResults,
                ColorCounts = CalculateColorCount(finalResults, options),
                SizeCounts = CalculateSizeCount(finalResults, options)
            };
        }

        private List<ColorCount> CalculateColorCount(List<Shirt> finalResults, SearchOptions searchOptions)
        {
            List<ColorCount> colorCounts = new List<ColorCount>(Color.All.Count);

            foreach(var color in Color.All)
            {
                // Avoiding variable capture using a temp variable
                var tempColor = color;
                var colorCount = new ColorCount()
                {
                    Color = color,
                    Count = finalResults.Count(y => searchOptions.Colors.Any() && y.Color.Id == tempColor.Id)
                };

                colorCounts.Add(colorCount);
            }

            return colorCounts;
        }

        private List<SizeCount> CalculateSizeCount(List<Shirt> finalResults, SearchOptions searchOptions)
        {
            List<SizeCount> sizeCounts = new List<SizeCount>(Size.All.Count);

            foreach (var size in Size.All)
            {
                // Avoiding variable capture using a temp variable
                var tempSize = size;
                var sizeCount = new SizeCount()
                {
                    Size = size,
                    Count = finalResults.Count(y => searchOptions.Sizes.Any() && y.Size.Id == tempSize.Id)
                };

                sizeCounts.Add(sizeCount);
            }

            return sizeCounts;
        }
    }
}
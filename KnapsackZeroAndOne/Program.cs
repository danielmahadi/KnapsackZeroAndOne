using System;
using System.Collections.Generic;

namespace KnapsackZeroAndOne
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxWeight = 5;

            var items = new List<Item>
            {
                new Item {Weight = 3, Value = 5},
                new Item {Weight = 2, Value = 3},
                new Item {Weight = 1, Value = 4},
            };

            var valueMatrix = new int[items.Count, maxWeight +1];
            var keepMatrix = new bool[items.Count, maxWeight + 1];

            var cellAbove = new int[maxWeight + 1];

            for (var itemIdx = 0; itemIdx < items.Count; itemIdx++)
            {
                var currentItem = items[itemIdx];

                for (var weight = 1; weight <= maxWeight; weight++)
                {
                    var chosenVal = 0;
                    var keep = false;

                    var withinWeightLimit = currentItem.Weight <= weight;

                    var valueOfCellAbove = cellAbove[weight];

                    if (withinWeightLimit)
                    {
                        var remainingWeight = weight - currentItem.Weight;
                        var otherPossibleVal = cellAbove[remainingWeight];

                        var finalPossibleVal = currentItem.Value + otherPossibleVal;

                        if (valueOfCellAbove <= finalPossibleVal)
                        {
                            chosenVal = finalPossibleVal;
                            keep = true;
                        }
                        else
                        {
                            chosenVal = valueOfCellAbove;
                            keep = false;
                        }
                    }
                    else
                    {
                        chosenVal = valueOfCellAbove;
                        keep = false;
                    }

                    valueMatrix[itemIdx, weight] = chosenVal;
                    keepMatrix[itemIdx, weight] = keep;
                }

                for (var i = 0; i < maxWeight; i++)
                {
                    cellAbove[i] = valueMatrix[itemIdx, i];
                }
            }

            Console.WriteLine("Value Matrix");
            Print(valueMatrix);

            Console.WriteLine("Keep Matrix");
            Print(keepMatrix);

            Console.WriteLine("Selected Values");
            var selectedItems = Optimize(items, keepMatrix, maxWeight);
            foreach (var selectedItem in selectedItems)
            {
                Console.WriteLine($"{ selectedItem.Value }");
            }

            Console.ReadKey();
        }

        private static IList<Item> Optimize(IList<Item> items, bool[,] keepMatrix, int maxWeight)
        {
            var selectedItems = new List<Item>();
            var remainingWeight = maxWeight;

            for (var i = items.Count -1; i >= 0; i--)
            {
                for (var w = remainingWeight; w > 0; w--)
                {
                    if (w == remainingWeight)
                    {
                        var isKeep = keepMatrix[i, w];

                        if (!isKeep)
                            break;

                        var currentItem = items[i];
                        selectedItems.Add(currentItem);
                        remainingWeight -= currentItem.Weight;
                        
                    }

                    break;
                }

            }

            return selectedItems;
        }

        private static void Print<T>(T[, ] arr)
        {
            var rowLength = arr.GetLength(0);
            var colLength = arr.GetLength(1);

            for (var i = 0; i < rowLength; i++)
            {
                for (var j = 1; j < colLength; j++)
                {
                    Console.Write($"{arr[i, j]} | ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }

    class Item
    {
        public int Weight { get; set; }
        public int Value { get; set; }
    }
}

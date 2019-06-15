using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Extensions
{
    public static class RandomExtensions
    {
        private static readonly System.Random random = new System.Random();

        public static T GetRandom<T>(this T[] array)
        {
            return array[random.Next(0, array.Length)];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Get uniform random number from input min and max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumber(this int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Get random bool with chance of true/false with stated ints.
        /// </summary>
        /// <param name="_true"></param>
        /// <param name="_false"></param>
        /// <returns></returns>
        public static bool GetRandomBool(this int _true, int _false)
        {
            int prob = random.Next(_true + _false);

            bool _bool = (prob >= _true) ? false : true;

            return _bool;
        }
    }
}
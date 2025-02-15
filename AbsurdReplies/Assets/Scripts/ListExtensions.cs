﻿using System;
using System.Collections.Generic;

namespace AbsurdReplies
{
    public static class ListExtensions
    {
        private static Random _random = new Random();
        
        public static void Shuffle<T>(this IList<T> list)
        {
            for(var i=list.Count; i > 0; i--)
                list.Swap(0, _random.Next(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
            var index = _random.Next(0, list.Count - 1);
            return list[index];
        }
    }
}
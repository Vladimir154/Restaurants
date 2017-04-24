using System;
using System.Text.RegularExpressions;

namespace Restaurants.RegularExpressions
{
    public class RegularExpressions
    {
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
    }
}

using System.Text;
using DepthChart.Models;

namespace DepthChart.Extensions
{
    public static class KeyValuePairExtensions
    {
        /// <summary>
        ///     Formats a KeyValuePair of string and List
        ///     I've done this because the question said "Prints out
        ///     all depth chart positions", suggesting a text output.
        ///     I'm sure that's what you're after, but it's an idea...
        /// </summary>
        public static string ToFormattedString(this KeyValuePair<string, List<Player>> kvp)
        {
            var sb = new StringBuilder("[");

            for (var i = 0; i < kvp.Value.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(kvp.Value[i].PlayerId);
            }

            sb.Append(']');

            return $"{kvp.Key}: {sb}";
        }
    }
}
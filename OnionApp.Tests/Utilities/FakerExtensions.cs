using Bogus;
using System.Linq;

namespace OnionApp.Tests.Utilities
{
    public static class FakerExtensions
    {
        public static string GetStringWithLength(this Faker faker, int length)
        {
            if(length <= 0)
            {
                return string.Empty;
            }

            var chars = Enumerable.Repeat('#', length);
            return string.Concat(chars);
        }
    }
}

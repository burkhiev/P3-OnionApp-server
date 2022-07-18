using System;

namespace OnionApp.Tests.Exceptions
{
    internal class InvalidFixtureDataException : InvalidOperationException
    {
        public InvalidFixtureDataException()
            : base("One of the fixture data may be invalid. " +
                  "Check fixture and test class for data leaks or for incorrect initial data.")
        {

        }
    }
}

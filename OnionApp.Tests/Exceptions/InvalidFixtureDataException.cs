using System;

namespace OnionApp.Tests.Exceptions
{
    internal class InvalidFixtureDataException : InvalidOperationException
    {
        public InvalidFixtureDataException()
            : base("One of the fixture db rows count is invalid. " +
                  "Check fixture constructor or Dispose method for data leaks.")
        {

        }
    }
}

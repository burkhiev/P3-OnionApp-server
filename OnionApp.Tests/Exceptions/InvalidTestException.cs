using System;

namespace OnionApp.Tests.Exceptions
{
    public class InvalidTestException : InvalidOperationException
    {
        public InvalidTestException(Type testClass, string methodName)
            : base($"Invalid test operation." +
                  $"\n  Class: {testClass.FullName}." +
                  $"\n  Method: {testClass.GetMethod(methodName).Name}.")
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    public class HelloWorldService : IHelloWorld
    {
        #region IHelloWorld Members

        public string HelloWorld(string message)
        {
            return "Hello world " + message;
        }

        #endregion
    }
}

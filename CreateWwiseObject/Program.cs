using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace CreateWwiseObject
{
    class Program
    {
        const string serverAddress = "ws://127.0.0.1:8080/waapi";

        static void Main(string[] args)
        {
            DefaultWampChannelFactory factory = new DefaultWampChannelFactory();
            IWampChannel channel = factory.CreateJsonChannel(serverAddress, "realm1");
            channel.Open().Wait();

            IWampRealmProxy realmProxy = channel.RealmProxy;

            CreateObject("{D9E5537B-DD66-4790-A1FF-74C0CC032C9D}", "Sound", "Test_Sound", realmProxy);
        }

        public static void CreateObject(string parent, string type, string name, IWampRealmProxy realmProxy)
        {
            IDictionary<string, object> keywordArguments = new Dictionary<string, object>();

            keywordArguments.Add("parent", parent);
            keywordArguments.Add("type", type);
            keywordArguments.Add("name", name);

            realmProxy.RpcCatalog.Invoke(
                new CreateObjectCallback(),
                new CallOptions(),
                "ak.wwise.core.object.create",
                new object[] { },
                keywordArguments);
        }

        public class CreateObjectCallback : IWampRawRpcOperationClientCallback
        {
            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
            }

            // Other method overloads are never used: WAAPI always sends keyword arguments
            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords) { }
            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error) { }
            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments) { }
            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details) { }
            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments) { }
        }
    }
}

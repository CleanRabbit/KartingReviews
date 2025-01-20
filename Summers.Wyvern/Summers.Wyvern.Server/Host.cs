using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Summers.Wyvern.Server.MongoDb;
using Summers.Wyvern.Server.MongoDb.Database;
using Summers.Wyvern.SpeechManager;

namespace Summers.Wyvern.Server
{
	public class Host
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IDataAccessController _dataAccessController = new WyvernDataAccessController();

        public Host(IConfiguration configuration)
        {
            _dataAccessController.Connect(configuration[Identifiers.MongoDbHostConfigId], int.Parse(configuration[Identifiers.MongoDbPortConfigId]), configuration[Identifiers.MongoDbDatabaseNameConfigId]);
        }

        public async Task Start()
        {
            Console.WriteLine("Host Started");
        }

        public async Task ShutDown()
        {

        }
    }
}

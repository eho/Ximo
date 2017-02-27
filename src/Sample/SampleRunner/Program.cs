using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Commands;
using Sample.Domain;
using Sample.Domain.Data;
using Sample.ReadModel;
using Ximo.Cqrs;
using Ximo.DependencyInjection;
using Ximo.Domain;
using Ximo.Utilities;

namespace SampleRunner
{
    internal class Program
    {
        private static void Main()
        {
            var serviceProvider = Bootstrap();

            var commandBus = serviceProvider.GetRequiredService<ICommandBus>();
            SimulateAccountProcessing(commandBus);

            Console.WriteLine("Sample completed!");
            Console.ReadKey();
        }

        private static void SimulateAccountProcessing(ICommandBus commandBus)
        {
            var newAccountId = Guid.NewGuid();
            var createAccount = new CreateAccount(newAccountId, "Omar", @"Besiso", "ThoughtDesign",
                RandomGenerator.GenerateRandomEmail());

            try
            {
                commandBus.Send(createAccount);

                //Simulate 6 snapshots
                for (var i = 0; i < 60; i++)
                {
                    var updateAccount = new UpdateAccountAddress(newAccountId, $"Test {i}", null, null, null, null,
                        "Australia");
                    commandBus.Send(updateAccount);
                }

                var approveAccount = new ApproveAccount(newAccountId, "Omar Besiso");
                commandBus.Send(approveAccount);

                var deleteAccount = new DeleteAccount(newAccountId, "Testing");
                commandBus.Send(deleteAccount);

                var reinstateAccount = new ReinstateAccount(newAccountId);
                commandBus.Send(reinstateAccount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IServiceProvider Bootstrap()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            IConfiguration configuration = builder.Build();
            serviceCollection.AddSingleton(configuration);

            serviceCollection.AddLogging();

            serviceCollection.RegisterDefaultCommandBus();
            serviceCollection.RegisterDefaulDomainEventBus();
            serviceCollection.RegisterDefaultQueryProcessor();

            serviceCollection.LoadModule<DomainModule>();
            serviceCollection.LoadModule<DomainDataModule>(configuration);
            serviceCollection.LoadModule<ReadModelModule>(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<ILoggerFactory>().AddConsole();

            var domainEventBus = serviceProvider.GetRequiredService<IDomainEventBus>();

            DomainModule.RegisterSubscriptions(domainEventBus);
            ReadModelModule.RegisterSubscriptions(domainEventBus);

            return serviceProvider;
        }
    }
}
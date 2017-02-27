using System.Linq;
using Sample.DomainEvents;
using Ximo.Domain;

namespace Sample.ReadModel.EventHandlers
{
    internal class AddressUpdatedHandler : IDomainEventHandler<AddressUpdated>
    {
        private readonly ReadModelContext _modelContext;

        public AddressUpdatedHandler(ReadModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public void Handle(AddressUpdated @event)
        {
            var account = _modelContext.AccountDetails.First(x => x.AccountId == @event.AccountId);
            account.SetAddressDetails(@event);
            _modelContext.SaveChanges();
        }
    }
}
using Ximo.Domain;
using Ximo.Validation;

namespace Sample.Domain.ValueObjects
{
    public class AccountStatus : ValueObject<AccountStatus>
    {
        private string _approvedBy;
        private string _deletedReason;

        public AccountStatus(bool isApproved, string approvedBy, bool isDeleted, string deletedReason)
        {
            IsApproved = isApproved;
            _approvedBy = approvedBy;
            IsDeleted = isDeleted;
            _deletedReason = deletedReason;
        }

        public bool IsApproved { get; private set; }

        public string ApprovedBy
        {
            get { return _approvedBy; }
            // ReSharper disable once UnusedMember.Local
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _approvedBy = value;
            }
        }

        public bool IsDeleted { get; private set; }

        public string DeletedReason
        {
            get { return _deletedReason; }
            // ReSharper disable once UnusedMember.Local
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _deletedReason = value;
            }
        }
    }
}
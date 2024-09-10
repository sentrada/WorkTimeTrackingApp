using CQRS.Core.Domain;
using UserManagement.Common.Events;

namespace UserManagement.CommandService.Domain.Aggregates
{
    public class UserAggregate : AggregateRoot
    {
        private bool _isDeleted;

        public bool IsDeleted
        {
            get => _isDeleted;
            private set => _isDeleted = value;
        }

        public UserAggregate() { }

        public UserAggregate(Guid id, string name, string email, string password)
        {
            
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Name cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Email cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Password cannot be null or empty.");
            }

            RaiseEvent(new UserCreatedEvent
            {
                Id = id,
                UserName = name, 
                Email = email, 
                Password = password
            });
        }

        public void Apply(UserCreatedEvent @event)
        {
            _id = @event.Id;
            _isDeleted = false;
        }
        
        public void UpdateUser(string name, string email)
        {
            if (_isDeleted)
            {
                throw new InvalidOperationException("You cannot update a deleted user.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Name cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Email cannot be null or empty.");
            }

            RaiseEvent(new UserUpdatedEvent
            {
                Id = _id,
                UserName = name, 
                Email = email
            });
        }
        
        public void Apply(UserUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void DeleteUser()
        {
            if (_isDeleted)
            {
                throw new InvalidOperationException("User is already deleted.");
            }

            RaiseEvent(new UserDeletedEvent
            {
                Id = _id
            });
        }
        
        public void Apply(UserDeletedEvent @event)
        {
            _id = @event.Id;
            _isDeleted = true;
        }
    }
}

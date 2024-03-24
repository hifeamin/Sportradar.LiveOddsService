using System;

namespace Sportradar.LiveOddsService.Domain.Exceptions {
    public class ItemAlreadyExistException: Exception {
        public ItemAlreadyExistException() : base() { }

        public ItemAlreadyExistException(string message) : base(message) { }
    }
}

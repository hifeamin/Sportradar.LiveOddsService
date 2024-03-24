using System;

namespace Sportradar.LiveOddsService.Domain.Exceptions {
    public class ItemAlreadyExistException<T> : Exception {
        public ItemAlreadyExistException(string message, T existItem) : base(message) {
            ExistItem = existItem;
        }

        public T ExistItem { get; set; }
    }
}

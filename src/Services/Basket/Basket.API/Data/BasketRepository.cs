namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDocumentSession _session;

        public BasketRepository(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await _session.LoadAsync<ShoppingCart>(userName, cancellationToken);

            return basket == null ? throw new BasketNotFoundException(userName) : basket;
        }

        public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            _session.Store(basket);
            await _session.SaveChangesAsync(cancellationToken);

            return basket;
        }
        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            _session.Delete<ShoppingCart>(userName);
            await _session.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

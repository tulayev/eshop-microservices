namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResult(ShoppingCart Cart);
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

    public class GetBasketHandler(IBasketRepository basketRepository) 
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasketAsync(query.UserName);
            
            return new GetBasketResult(basket);
        }
    }
}

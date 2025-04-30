namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketResult(string UserName);

    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository basketRepository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var cart = command.Cart;

            await basketRepository.StoreBasketAsync(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}

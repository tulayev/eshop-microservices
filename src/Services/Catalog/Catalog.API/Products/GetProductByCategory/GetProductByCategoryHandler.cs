namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    internal class GetProductByCategoryQueryHandler(
        IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext context;
        public EFStoreRepository(StoreDbContext dbContext)
        {
            context = dbContext;
        }
        public IQueryable<Product> Products => context.Products;
    }
}

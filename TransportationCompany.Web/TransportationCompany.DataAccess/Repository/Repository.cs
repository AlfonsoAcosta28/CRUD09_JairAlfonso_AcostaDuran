namespace TransportationCompany.DataAccess.Repository
{
    public class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class, new()
    {
        private readonly TransportationCompanyContext _context;
        protected TransportationCompanyContext Context { get => _context; }
        public Repository(TransportationCompanyContext TransportationCompanyContext)
        {
            _context = TransportationCompanyContext;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");

            try
            {
                _context.ChangeTracker.Clear();
                await _context.AddAsync(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            _context.ChangeTracker.Clear();
            _context.Remove<TEntity>(entity);
            await _context.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            try
            {
                var list = _context.Set<TEntity>();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"could not retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException($"{nameof(entity)} must not be null");

            try
            {
                _context.ChangeTracker.Clear();
                _context.Update(entity);
               // await _context.SaveChangesAsync();
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

    }
}

using Post.Domain.Entities.Post;

namespace Post.Infrastructure.Persistence.Repositories;

public class PostRepository : CrudRepository<PostEntity>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    { }
}
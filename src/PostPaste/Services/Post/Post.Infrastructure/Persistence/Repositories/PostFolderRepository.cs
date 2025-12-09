using Post.Domain.Entities.PostFolder;

namespace Post.Infrastructure.Persistence.Repositories;

public class PostFolderRepository : CrudRepository<PostFolderEntity>, IPostFolderRepository
{
    public PostFolderRepository(ApplicationDbContext context) : base(context)
    { }
}
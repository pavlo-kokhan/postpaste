using Post.Domain.Entities.Abstract;

namespace Post.Domain.Entities.Post;

public interface IPostRepository : ICrudRepository<PostEntity>;
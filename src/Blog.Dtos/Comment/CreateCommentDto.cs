namespace Blog.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string PostId { get; set; }
        public string Content { get; set; }
        public string CommentId { get; set; }
    }
}

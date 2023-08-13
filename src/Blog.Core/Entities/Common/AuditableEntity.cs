namespace Blog.Core.Entities.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime DateTime { get; set; }
    }
}

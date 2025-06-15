namespace Hvt.Data.Models.Base
{
    public interface IEntityWithTimestamps : IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

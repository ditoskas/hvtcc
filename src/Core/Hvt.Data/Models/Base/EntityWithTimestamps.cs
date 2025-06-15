namespace Hvt.Data.Models.Base
{
    public class EntityWithTimestamps : Entity, IEntityWithTimestamps
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

namespace WebAPIEgitimi.HastaKabulWebAPI.Abstractions
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.CreateVersion7();
            CreatedAt = DateTimeOffset.UtcNow; // CreateAt property'sini constructor içinde başlatıyoruz. Ileride InMemory de test edebilmek icin
        }

        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}

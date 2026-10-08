namespace WebAPIEgitimi.HastaKabulWebAPI.Abstractions
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.CreateVersion7();
            CreateAt = DateTimeOffset.UtcNow; // CreateAt property'sini constructor içinde başlatıyoruz. Ileride InMemory de test edebilmek icin
        }

        public Guid Id { get; set; }

        public DateTimeOffset CreateAt { get; set; }

        public DateTimeOffset? UpdateAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeleteAt { get; set; }
    }
}

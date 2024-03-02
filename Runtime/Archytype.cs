namespace CZToolKit.ECS
{
    public unsafe struct Archetype
    {
        public TypeInfo* types;
        public int typeCount;

        public int hash;

        public Archetype(TypeInfo* types, int count)
        {
            this.types = types;
            this.typeCount = count;

            hash = 0;
            for (int i = 0; i < count; i++)
            {
                hash ^= (types + i)->id;
            }
        }
    }

    public unsafe struct Chunk
    {
        public Archetype* archetype;
        public int capacity;
        public int count;
    }

    public unsafe class EntityInChunk
    {
        public Chunk* chunk;
        public int index;
    }
}
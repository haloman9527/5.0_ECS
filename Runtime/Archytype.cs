using CZToolKit.UnsafeEx;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public unsafe struct Archetype
    {
        public TypeInfo* types;
        public int typesCount;

        public NativeArray<Chunk> chunks;

        public int hash;

        public Archetype(TypeInfo* types, int count)
        {
            this.types = types;
            this.typesCount = count;
            
            this.chunks = new NativeArray<Chunk>();

            this.hash = 0;
            for (int i = 0; i < count; i++)
            {
                this.hash ^= (types + i)->id;
            }
        }
    }

    public unsafe struct Chunk
    {
        public const int kChunkSize = 16 * 1024;
        
        public Archetype* archetype;
        public int capacity;
        public int count;
    }

    public unsafe class TypeInArchetype
    {
        public Archetype* archetype;
        public int typeIndex;
    }

    public unsafe class EntityInChunk
    {
        public Chunk* chunk;
        public int indexInChunk;
    }
}
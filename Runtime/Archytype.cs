using System;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public unsafe struct Archetype : IDisposable
    {
        public const int DEFAULT_CHUNKS_LENGTH = 4;

        public TypeInfo* sortedTypes;
        public int typesCount;
        public int archetypeHash;
        public int chunkSize;
        public NativeParallelHashMap<int, int> typeInArchetypeIndexMap;
        public NativeArray<Chunk> chunks;

        public Archetype(TypeInfo* sortedTypes, int count)
        {
            this.sortedTypes = sortedTypes;
            this.typesCount = count;

            this.archetypeHash = 0;
            this.chunkSize = 0;

            this.typeInArchetypeIndexMap = new NativeParallelHashMap<int, int>(count, Allocator.Persistent);
            this.chunks = new NativeArray<Chunk>(DEFAULT_CHUNKS_LENGTH, Allocator.Persistent);

            for (int i = 0; i < count; i++)
            {
                var typeInfo = (sortedTypes + i);
                this.archetypeHash ^= typeInfo->id;
                this.chunkSize += typeInfo->componentSize;
                this.typeInArchetypeIndexMap[typeInfo->id] = i;
            }
        }

        public int GetTypeInArchetypeIndex(int typeId)
        {
            if (!typeInArchetypeIndexMap.TryGetValue(typeId, out var typeInArchetypeIndex))
            {
                return -1;
            }

            return typeInArchetypeIndex;
        }

        public int GetTypeInArchetypeIndex(TypeInfo typeInfo)
        {
            var typeId = typeInfo.id;
            return GetTypeInArchetypeIndex(typeId);
        }

        public int GetTypeInArchetypeIndex(Type type)
        {
            var typeId = TypeManager.GetTypeId(type);
            return GetTypeInArchetypeIndex(typeId);
        }

        public void Dispose()
        {
            typeInArchetypeIndexMap.Dispose();
            chunks.Dispose();
        }
    }

    public unsafe struct Chunk
    {
        public const int CHUNK_SIZE = 16 * 1024;

        public Archetype* archetype;
        
        public int capacity;
        public int count;

        public Chunk(Archetype* archetype)
        {
            this.archetype = archetype;
            this.capacity = CHUNK_SIZE / (archetype->chunkSize + sizeof(Entity));
            this.count = 0;
        }
    }

    public unsafe class TypeInArchetype
    {
        public Archetype* archetype;
        public int typeId;
        public int indexInChunk;
        public int offsetInChunk;
    }

    public unsafe class EntityInChunk
    {
        public Chunk* chunk;
        public int indexInChunk;
    }
}
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Atom.ECS
{
    public unsafe struct Archetype : IDisposable
    {
        public const int DEFAULT_CHUNKS_LENGTH = 4;

        public TypeInfo* sortedTypes;
        public int* offsets;
        public int typesCount;
        public int archetypeHash;
        public int componentsSize;
        public UnsafeParallelHashMap<int, int> typeInArchetypeIndexMap;
        public UnsafePtrList<Chunk> chunks;
        public UnsafePtrList<Chunk> chunksWithEmptySlots;

        // public Archetype(TypeInfo* sortedTypes, int count)
        // {
        //     this.sortedTypes = sortedTypes;
        //     this.typesCount = count;
        //
        //     this.archetypeHash = 0;
        //     this.chunkSize = 0;
        //
        //     this.typeInArchetypeIndexMap = new UnsafeParallelHashMap<int, int>(count, Allocator.Persistent);
        //     this.chunks = new UnsafeList<Chunk>(DEFAULT_CHUNKS_LENGTH, Allocator.Persistent);
        //
        //     for (int i = 0; i < count; i++)
        //     {
        //         var typeInfo = (sortedTypes + i);
        //         this.archetypeHash ^= typeInfo->id;
        //         this.chunkSize += typeInfo->componentSize;
        //         this.typeInArchetypeIndexMap[typeInfo->id] = i;
        //     }
        // }

        internal Chunk* GetExistingChunkWithEmptySlots()
        {
            if (chunksWithEmptySlots.Length != 0)
            {
                var chunk = chunksWithEmptySlots.Ptr[0];
                return chunk;
            }

            return null;
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

        public void Reset()
        {
            typeInArchetypeIndexMap.Clear();
            chunks.Clear();
        }

        public void Dispose()
        {
            typeInArchetypeIndexMap.Dispose();
            chunks.Dispose();
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Chunk
    {
        public const int CHUNK_SIZE = 16 * 1024;

        // NOTE: SequenceNumber is not part of the serialized header.
        //       It is cleared on write to disk, it is a global in memory sequence ID used for comparing chunks.
        public const int SERIALIZED_HEADER_SIZE = 40;
        public const int BUFFER_OFFSET = 64; // (must be cache line aligned)
        public const int BUFFER_SIZE = CHUNK_SIZE - BUFFER_OFFSET;
        public const int MAXIMUM_ENTITIES_PER_CHUNK = BUFFER_SIZE / 8;

        [FieldOffset(0)] public Archetype* archetype;

        [FieldOffset(8)] public int capacity;

        [FieldOffset(12)] public int count;

        /// <summary>
        /// The index of this Chunk within its ArchetypeChunkData's chunk list
        /// </summary>
        [FieldOffset(16)] public int listIndex;

        // Special chunk behaviors
        [FieldOffset(20)] public uint flags;

        // SequenceNumber is a unique number for each chunk, across all worlds. (Chunk* is not guranteed unique, in particular because chunk allocations are pooled)
        [FieldOffset(SERIALIZED_HEADER_SIZE)] public ulong sequenceNumber;

        [FieldOffset(BUFFER_OFFSET)] public fixed byte buffer[BUFFER_SIZE];

        public int UnusedCount => capacity - count;
    }

    public unsafe struct TypeInArchetype
    {
        public Archetype* archetype;
        public int typeId;
        public int indexInChunk;
        public int offsetInChunk;
    }

    public unsafe struct EntityInChunk
    {
        public Chunk* chunk;
        public int indexInChunk;
    }
}
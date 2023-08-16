#region 注 释

/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */

#endregion

namespace CZToolKit.ECS
{
    public unsafe struct Chunk
    {
        public const int CHUNK_SIZE = 1024 * 16;
        
        public Archetype* archetype;
        public int inArchetypeIndex;
    }
}
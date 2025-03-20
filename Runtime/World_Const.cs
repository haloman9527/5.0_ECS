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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

namespace Atom.ECS
{
    public partial class World
    {
        public const int ENTITY_ID_LENGTH = 48;
        public const int COMPONENT_ID_LENGTH = 16;
        
        public const ulong MAX_ENTITIES = ulong.MaxValue & ulong.MaxValue >> (64 - ENTITY_ID_LENGTH);
        public const ulong MAX_COMPONENTS = ulong.MaxValue & ulong.MaxValue >> (64 - COMPONENT_ID_LENGTH);
    }
}
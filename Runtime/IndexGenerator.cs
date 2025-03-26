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

using System.Threading;

namespace Atom.ECS
{
    public class IdGenerator
    {
        private long index;

        public uint Next()
        {
            return (uint)Interlocked.Increment(ref index);
        }
        
        public void Reset()
        {
            index = 0;
        }
    }
}
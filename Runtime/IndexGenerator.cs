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
    public class IndexGenerator
    {
        private uint index = 1;

        public uint Next()
        {
            return index++;
        }
        
        public void Reset()
        {
            index = 1;
        }
    }
}
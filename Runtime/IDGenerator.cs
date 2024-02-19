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

namespace CZToolKit.ECS
{
    public class IndexGenerator
    {
        private int index;

        public IndexGenerator(int startIndex = 1)
        {
            this.index = startIndex;
        }

        public int Next()
        {
            return index++;
        }
    }
}
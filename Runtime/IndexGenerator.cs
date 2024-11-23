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

namespace Jiange.ECS
{
    public class IndexGenerator
    {
        private int index = 1;

        public int Next()
        {
            return index++;
        }
        
        public void Reset()
        {
            index = 1;
        }
    }
}
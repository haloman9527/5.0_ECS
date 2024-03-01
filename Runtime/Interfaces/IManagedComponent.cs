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
    public interface IManagedComponent : IComponent
    {
        int Id { get; set; }

        void SetValue(object value);

        object GetValue();
    
        void Alloc(World world);

        void Release();
    }
}

// 如果是IManagedComponent，在添加这个组件的时候，分配一个Id
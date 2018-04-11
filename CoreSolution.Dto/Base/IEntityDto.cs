namespace CoreSolution.Dto.Base
{
    /// <summary>
    /// 主键为Int的EntityDto接口
    /// </summary>
    public interface IEntityDto : IEntityDto<int>
    {
    }

    /// <summary>
    /// 泛型主键EntityDto接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntityDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}

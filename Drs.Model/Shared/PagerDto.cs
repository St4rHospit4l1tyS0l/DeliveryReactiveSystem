namespace Drs.Model.Shared
{
    public class PagerDto<T>
    {
        public T Data { get; set; }
        public PagerModel Pager { get; set; }
    }
}

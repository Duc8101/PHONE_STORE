namespace Common.Paginations
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
            List = new List<T>();
        }

        public int PageSelected { get; set; }
        public List<T> List { get; set; }
        public int NumberPage { get; set; }

    }
}

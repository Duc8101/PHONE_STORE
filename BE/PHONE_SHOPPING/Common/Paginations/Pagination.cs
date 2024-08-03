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
        public string PRE_URL { get; set; } = string.Empty;
        public string NEXT_URL { get; set; } = string.Empty;
        public string FIRST_URL { get; set; } = string.Empty;
        public string LAST_URL { get; set; } = string.Empty;

    }
}

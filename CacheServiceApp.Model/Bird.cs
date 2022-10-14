namespace CacheServiceApp.Model
{
    public class Bird
    {
        public string Uid { get; set; }
        public Name Name { get; set; }
        public Images Images { get; set; }
        public Links _Links { get; set; }
        public int Sort { get; set; }
    }
}

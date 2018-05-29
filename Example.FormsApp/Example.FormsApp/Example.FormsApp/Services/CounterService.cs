namespace Example.FormsApp.Services
{
    public class CounterService
    {
        private int counter;

        public int IncrementAndGet()
        {
            return ++counter;
        }
    }
}

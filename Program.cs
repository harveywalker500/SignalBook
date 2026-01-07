namespace SignalBook
{
    public static class Program
    {
        private static TimeInterface _timeInterface;

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            _timeInterface = new TimeInterface();

        }
    }
}
using SignalBook.Controllers;

namespace SignalBook
{
    public static class Program
    {
        private static TimeController _timeInterface;

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            _timeInterface = new TimeController();

        }
    }
}
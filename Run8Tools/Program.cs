using Run8Tools.Commands;

namespace Run8Tools
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no subcommand specified");
                return 1;
            }

            string mode = args[0];

            switch (mode.ToLower())
            {
                case "avatar":
                    return new Avatar().Main(args);
                case "texture":
                    return new Texture().Main(args);
                default:
                    Console.WriteLine("Unknown mode");
                    return 1;
            }
        }
    }
}
using System;

namespace teamstairwell
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RNSEB game = new RNSEB())
            {
                game.Run();
            }
        }
    }
}


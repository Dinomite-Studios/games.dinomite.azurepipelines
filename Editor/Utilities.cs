namespace Dinomite.AzurePipelines
{
    public static class Utilities
    {
        public static bool CommandLineArgumentExists(string argumentName)
        {
            var commandLineArguments = System.Environment.GetCommandLineArgs();

            for (var i = 0; i < commandLineArguments.Length; i++)
            {
                if (string.Equals(commandLineArguments[i], argumentName))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetCommandLineArgumentValue(string argumentName, out string value)
        {
            var commandLineArguments = System.Environment.GetCommandLineArgs();

            for (var i = 0; i < commandLineArguments.Length; i++)
            {
                if (string.Equals(commandLineArguments[i], argumentName) && i + 1 < commandLineArguments.Length)
                {
                    value = commandLineArguments[i + 1];
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}

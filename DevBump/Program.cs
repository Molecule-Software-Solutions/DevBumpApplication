namespace DevBump;

public class Program
{
    public static void Main(string[] args)
    {
        // Guard
        if(args.Count() == 0 || string.IsNullOrEmpty(args[0]))
        {
            Console.WriteLine("You must provide a file path and option flag");
            Console.WriteLine("Try DevBump --help");
            return; 
        }

        // Help option for flag on argument 0
        if(args[0] == "--help" || args[0] == "--HELP")
        {
            Console.WriteLine("To use: DevBump <Path to CSPROJ file> <Option flag>");
            Console.WriteLine("Option Flags");
            Console.WriteLine("-------------");
            Console.WriteLine("/p increases the patch version by 1");
            Console.WriteLine("/r increases the release version by 1 and resets the patch version to 0");
            Console.WriteLine("/n increases the minor release version by 1 and resets release and patch to 0");
            Console.WriteLine("/m increases the major release version by 1 and resets minor, release, and patch to 0"); 
            return; 
        }

        // FLags
        bool changesMade = false; 

        // Construct a new XML Document to read the data into
        XmlDocument document = new XmlDocument();
        document.Load(args[0]);
        var documentElement = document.DocumentElement;
        var documentElementChildren = documentElement.ChildNodes;
        foreach (XmlNode innerNode in documentElementChildren)
        {
            if (innerNode.HasChildNodes)
            {
                foreach (XmlNode item in innerNode)
                {
                    if (item.Name == "AssemblyVersion")
                    {
                        Console.WriteLine("Found the assembly version node");
                        Console.WriteLine($"Current Value: {item.InnerText}");
                        var values = item.InnerText.Split('.');
                        int major = Convert.ToInt32(values[0]);
                        int minor = Convert.ToInt32(values[1]);
                        int release = Convert.ToInt32(values[2]);
                        int patch = Convert.ToInt32(values[3]);
                        Console.WriteLine($"Major: {major} - Minor: {minor} - Release: {release} - Patch: {patch}");
                        if(args[1] == "/p")
                        {
                            Console.WriteLine("Moving patch version by 1");
                            patch += 1;
                        }
                        if(args[1] == "/r")
                        {
                            Console.WriteLine("Moving release version by 1 and resetting patch");
                            release += 1;
                            patch = 0; 
                        }
                        if(args[1] == "/n")
                        {
                            Console.WriteLine("Moving minor version by 1 and resetting release and patch");
                            minor += 1;
                            release = 0;
                            patch = 0;
                        }
                        if(args[1] == "/m")
                        {
                            Console.WriteLine("Moving major version by 1 and resetting minor, release, and patch");
                            major += 1;
                            minor = 0;
                            release = 0;
                            patch = 0;
                        }
                        item.InnerText = $"{major}.{minor}.{release}.{patch}";
                        Console.WriteLine($"The new assembly version is now: {item.InnerText}");
                        changesMade = true; 
                    }
                }
            }
        }
        if(changesMade)
        {
            Console.WriteLine("Saving changes...");
            document.Save(args[0]);
        }
        Console.WriteLine("Done");
    }
}

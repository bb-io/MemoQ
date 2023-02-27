
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models;

public class Application : IApplication
{
    public Application()
    {
        Name = "MQ application";
    }

    public string Name { get; set; }
}
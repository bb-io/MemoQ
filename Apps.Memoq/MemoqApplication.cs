using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Memoq;

public class MemoqApplication : IApplication
{
    public string Name
    {
        get => "memoQ";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}
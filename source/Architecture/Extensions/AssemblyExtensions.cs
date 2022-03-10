using System.Reflection;

namespace Architecture.Extensions;

public static class AssemblyExtensions
{
    public static FileInfo FileInfo(this Assembly assembly) => new(assembly.Location);
}

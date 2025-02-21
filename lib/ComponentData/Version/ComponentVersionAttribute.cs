namespace StudentPortal.ComponentData;

[AttributeUsage(AttributeTargets.All)]
public class ComponentVersionAttribute(string VersionString) : Attribute
{
    public ComponentVersion Version => ComponentVersion.Parse(VersionString, null);
}
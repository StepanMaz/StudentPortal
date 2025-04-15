namespace StudentPortal.PageEditor.Templates;

public interface IPageTemplate
{
    public IEnumerable<IComponentFactory> Factories { get; }
}
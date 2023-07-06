namespace AdvanceTechniques.Tests.Unit;
public class MyClassFixture : IDisposable
{

    public Guid Id { get; } = Guid.NewGuid();

    public void Dispose()
    {
        System.Console.WriteLine("MyClassFixture.Dispose");
    }
}

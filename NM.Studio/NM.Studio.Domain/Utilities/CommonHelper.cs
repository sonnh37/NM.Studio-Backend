using IdGen;

namespace NM.Studio.Domain.Utilities;

public static class CommonHelper
{
    private static readonly IdGenerator _idGenerator;

    static CommonHelper()
    {
        var epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var structure = new IdStructure(45, 2, 16);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));
        _idGenerator = new IdGenerator(1, options);
    }

    public static string GenerateId()
    {
        var id = _idGenerator.CreateId();
        return $"{id}";
    }
}

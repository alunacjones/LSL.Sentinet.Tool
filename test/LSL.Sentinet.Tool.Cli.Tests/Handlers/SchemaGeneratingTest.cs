using LSL.Sentinet.Tool.Cli.Configuration;
using NJsonSchema;

namespace LSL.Sentinet.Tool.Cli.Tests.Handlers;

public class SchemaGeneratingTest
{
    [Test]
    public void GenerateSchema()
    {
        var schema = JsonSchema.FromType<ConfigurationFile>();
        var json = schema.ToJson();
    }

    public class FullSchema : ConfigurationFile
    {
        public IEnumerable<VariableItem> Variables { get; set; } = [];
    }    
}
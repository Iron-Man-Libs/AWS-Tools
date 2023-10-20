using System.Text.Json;

namespace Aws.Tools.Message.Serialization
{
    public class SnakeCaseToCamelCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            string[] parts = name.Split('_');
            string camelCaseName = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                camelCaseName += char.ToUpper(parts[i][0]) + parts[i][1..];
            }

            return camelCaseName;
        }
    }
}

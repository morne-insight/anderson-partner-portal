using Intent.RoslynWeaver.Attributes;
using MediatR;
using System.IO;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Health.GetPath
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetPathQueryHandler : IRequestHandler<GetPathQuery, string>
    {
        [IntentManaged(Mode.Merge)]
        public GetPathQueryHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<string> Handle(GetPathQuery request, CancellationToken cancellationToken)
        {
            string path = "/home/aspnet/DataProtection-Keys";
            
            if (!Directory.Exists(path))
            {
                return $"Directory {path} does not exist.";
            }
            
            var xmlFiles = Directory.GetFiles(path, "*.xml");
            
            if (xmlFiles.Length == 0)
            {
                return $"Directory {path} exists but contains no XML files.";
            }
            
            // Read the first XML file found
            string xmlContent = await File.ReadAllTextAsync(xmlFiles[0], cancellationToken);
            return xmlContent;
        }
    }
}

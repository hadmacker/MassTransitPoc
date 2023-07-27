using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace GettingStarted.Services
{
    internal class EmbeddedAnswerService : IAnswerService
    {
        public EmbeddedAnswerService(Random random)
        {
            Random = random;
        }

        internal Random Random { get; }

        public string GetAnswer()
        {
            var answers = ReadEmbeddedResource("answers.txt", this.GetType().Assembly).Split(Environment.NewLine);
            var index = Random.Next(0, answers.Length);
            return answers[index];
        }

        private string ReadEmbeddedResource(string resourceName, Assembly assembly)
        {
            var resources = assembly.GetManifestResourceNames().Where(r => r.Contains(resourceName));
            if(!resources.Any())
            {
                throw new InvalidOperationException($"No resource matching partial name of '{resourceName}' was found in assembly {assembly.FullName}. Is the resource marked as 'Build Action: Embedded Resource' in the Visual Studio project?");
            }
            if(resources.Count() != 1)
            {
                throw new InvalidOperationException($"Multiple resources matching partial name of '{resourceName}' were found in assembly {assembly.FullName}");
            }
            using (Stream stream = assembly.GetManifestResourceStream(resources.First()))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        return content;
                    }
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS
{
  public class Converter
  {
    void Convert(){
      var csApplication = new CSharpAnalyze.ApplicationService.AnalyzeApplication();

      // Register Analyzed Event
      csApplication.Register<CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed>(csApplication, (ev) =>
      {
        var result = ConvertTS(ev);
        Console.WriteLine(result);
      });

      // C# Source Code
      var testBase = new TestBase();
      testBase.CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }", null);


      // Run C# Analyze
      csApplication.Analyze(string.Empty, testBase.Files);

#if DEBUG
      Console.ReadKey();
#endif

    }
    string ConvertTS(CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed analyzed)
    {
      var result = new StringBuilder();

      result.AppendLine($"------{analyzed.FilePath} => {analyzed.FilePath.Replace(".cs", ".ts", StringComparison.CurrentCulture)}------");

      result.AppendLine(ConvertTS((dynamic)analyzed.FileRoot.Members.First()));

      return result.ToString();
    }

    string ConvertTS(CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems.IItemClass item, int indent = 0)
    {
      var result = new StringBuilder();
      var indentSpace = " ";

      result.AppendLine($"{indentSpace}class {item.Name}{{");
      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }
  }
}

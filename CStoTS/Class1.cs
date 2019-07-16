using System;
using System.Linq;
using System.Text;

namespace CStoTS
{
  public class Class1
  {
    /// <summary>
    /// Entry Point Method
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    static int Main(string[] args)
    {
      
      var csApplication = new CSharpAnalyze.ApplicationService.AnalyzeApplication();

      // Register Analyzed Event
      csApplication.Register<CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed>(csApplication, (ev) =>
      {
        var class1 = new Class1();
        var result = class1.ConvertTS(ev);
        Console.WriteLine(result);
      });

      // C# Source Code
      var testBase = new TestBase();
      testBase.CreateFileData("test", string.Empty, @"public class Test
      {
      }", null);


      // Run C# Analyze
      csApplication.Analyze(string.Empty, testBase.Files);

#if DEBUG
      Console.ReadKey();
#endif

      return 0;
    }

    string ConvertTS(CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed analyzed){
      return ConvertTS((dynamic)analyzed.FileRoot.Members.First());
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

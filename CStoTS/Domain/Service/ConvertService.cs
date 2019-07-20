using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using CStoTS.Domain.Model;
using CStoTS.Infrastructure;

namespace CStoTS.Domain.Service
{
  /// <summary>
  /// C#・TypeScript変換サービス
  /// </summary>
  internal class ConvertService
  {
    
    public void Convert(string inputCSRoot, string outputTSRoot, ITSFileRepository outputRepository, ICSFileRepository inputRepository)
    {
      var csApplication = new CSharpAnalyze.ApplicationService.AnalyzeApplication();

      // Register Analyzed Event
      csApplication.Register<CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed>(csApplication, (ev) =>
      {
        var converter = new Converter();
        var result = converter.ConvertTS(ev);
      });

      // Run C# Analyze
      csApplication.Analyze(inputCSRoot, inputRepository);
    }
  }
}

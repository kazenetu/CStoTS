using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using CStoTS.Domain.Model;
using CStoTS.Domain.Model.Mode;
using CStoTS.Infrastructure;
using System;
using System.IO;

namespace CStoTS.Domain.Service
{
  /// <summary>
  /// C#・TypeScript変換サービス
  /// </summary>
  internal class ConvertService
  {
    /// <summary>
    /// 変換処理
    /// </summary>
    /// <param name="config">設定情報</param>
    /// <param name="outputRepository">出力用リポジトリインスタンス</param>
    /// <param name="inputRepository">入力用リポジトリインスタンス</param>
    public void Convert(Config config, ITSFileRepository outputRepository, ICSFileRepository inputRepository)
    {
      var csApplication = new CSharpAnalyze.ApplicationService.AnalyzeApplication();

      // Register Analyzed Event
      csApplication.Register<CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed>(csApplication, (ev) =>
      {
        var converter = new MainConverter();
        var result = converter.ConvertTS(ev, config);

        // output file
        var filePath = ev.FilePath.Replace(".cs", ".ts", StringComparison.CurrentCulture);
        outputRepository.WriteFile($"{Path.Combine(config.OutputTSRoot.Value, filePath)}", result);
      });

      // Run C# Analyze
      csApplication.Analyze(config.InputCSRoot.Value, inputRepository);

      // output fixed TypeScript files
      outputRepository.OutputFixedTypeScripts(config.OutputTSRoot.Value);

    }
  }
}

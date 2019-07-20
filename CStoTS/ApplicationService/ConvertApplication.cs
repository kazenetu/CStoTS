using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using CSharpAnalyze.Infrastructure;
using CStoTS.Domain.Service;
using CStoTS.Infrastructure;

namespace CStoTS.ApplicationService
{
  /// <summary>
  /// C#・TypeScript変換アプリケーション
  /// </summary>
  public class ConvertApplication
  {
    /// <summary>
    /// C#・TypeScript変換
    /// </summary>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    public void Convert(string inputCSRoot, string outputTSRoot)
    {
      Convert(inputCSRoot, outputTSRoot, new TSFileRepository());
    }

    /// <summary>
    /// C#・TypeScript変換
    /// </summary>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    /// <param name="outputRepository">出力用リポジトリインスタンス</param>
    public void Convert(string inputCSRoot, string outputTSRoot, ITSFileRepository outputRepository)
    {
      Convert(inputCSRoot, outputTSRoot, outputRepository, new CSFileRepository());
    }

    /// <summary>
    /// C#・TypeScript変換
    /// </summary>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    /// <param name="outputRepository">出力用リポジトリインスタンス</param>
    /// <param name="inputRepository">入力用リポジトリインスタンス</param>
    public void Convert(string inputCSRoot, string outputTSRoot, ITSFileRepository outputRepository, ICSFileRepository inputRepository)
    {
      var service = new ConvertService();
      service.Convert(inputCSRoot, outputTSRoot, outputRepository,inputRepository);
    }
  }
}

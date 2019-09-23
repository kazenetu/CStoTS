using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using CSharpAnalyze.Infrastructure;
using CStoTS.Domain.Model.Mode;
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
    /// <param name="config">設定情報</param>
    public void Convert(Config config)
    {
      Convert(config, new TSFileRepository());
    }

    /// <summary>
    /// C#・TypeScript変換
    /// </summary>
    /// <param name="config">設定情報</param>
    /// <param name="outputRepository">出力用リポジトリインスタンス</param>
    public void Convert(Config config, ITSFileRepository outputRepository)
    {
      Convert(config, outputRepository, new CSFileRepository());
    }

    /// <summary>
    /// C#・TypeScript変換
    /// </summary>
    /// <param name="config">設定情報</param>
    /// <param name="outputRepository">出力用リポジトリインスタンス</param>
    /// <param name="inputRepository">入力用リポジトリインスタンス</param>
    public void Convert(Config config, ITSFileRepository outputRepository, ICSFileRepository inputRepository)
    {
      var service = new ConvertService();
      service.Convert(config, outputRepository,inputRepository);
    }
  }
}

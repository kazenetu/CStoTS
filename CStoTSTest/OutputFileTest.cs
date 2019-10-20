using CStoTS;
using System.IO;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("ファイル出力テスト", nameof(OutputFileTest))]
  public class OutputFileTest : TestBase
  {
    /// <summary>
    /// テスト用ベースパス
    /// </summary>
    private const string basePath = "OutputFileTest";

    /// <summary>
    /// テスト用相対C#パス（入力）
    /// </summary>
    private const string csDPath = "cs";

    /// <summary>
    /// テスト用相対TypeScriptパス（出力）
    /// </summary>
    private const string tsDPath = "ts";

    /// <summary>
    /// テスト用ルートパス
    /// </summary>
    /// <remarks>コンストラクタで設定</remarks>
    private string rootPath;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OutputFileTest() : base()
    {
      // ルートパスの設定
      rootPath = Directory.GetCurrentDirectory();
      var rootPathIndex = rootPath.IndexOf("bin", System.StringComparison.CurrentCulture);
      if(rootPathIndex > 0)
      {
        rootPath = rootPath.Substring(0, rootPathIndex-1);
      }

    }

    #region ディレクトリ作成・取得

    /// <summary>
    /// ディレクトリ作成・取得
    /// </summary>
    /// <param name="targetPath">ディレクトリパス</param>
    /// <returns>ディレクトリパス</returns>
    private string CreatePath(string targetPath)
    {
      // すでにディレクトリが存在する場合は削除
      if (Directory.Exists(targetPath))
      {
        // フォルダをクリアする
        Directory.Delete(targetPath, true);
      }

      // ディレクトリ作成
      Directory.CreateDirectory(targetPath);

      return targetPath;
    }

    /// <summary>
    /// 入力パス（C#ソース）
    /// </summary>
    /// <param name="methodName">テストメソッド名</param>
    /// <returns>絶対パス</returns>
    private string GetInputPath(string methodName)
    {
      return CreatePath(Path.Combine(rootPath, basePath, csDPath, methodName));
    }

    /// <summary>
    /// 出力パス（TypeScriptソース）
    /// </summary>
    /// <param name="methodName">テストメソッド名</param>
    /// <returns>絶対パス</returns>
    private string GetOutputPath(string methodName)
    {
      return CreatePath(Path.Combine(rootPath, basePath, tsDPath, methodName));
    }
    #endregion

    #region ファイル出力

    /// <summary>
    /// ファイル出力
    /// </summary>
    /// <param name="path">絶対パス</param>
    /// <param name="fileName">ファイル名</param>
    /// <param name="source">ソースコード</param>
    private void CreateCSFile(string path, string fileName, string source)
    {
      var filePath = Path.Combine(path, fileName);

      // ファイル名にディレクトリが存在し、ディレクトリが作成されていない場合は作成
      var fileInDirectory = Path.GetDirectoryName(filePath);
      if (!string.IsNullOrEmpty(fileInDirectory))
      {
        Directory.CreateDirectory(Path.Combine(path, fileInDirectory));
      }

      // ファイル書き込み
      using (var fs = File.CreateText(filePath))
      {
        fs.Write(CreateCSCode(string.Empty, source));
      }

    }

    /// <summary>
    /// ファイル読み込み
    /// </summary>
    /// <param name="path">絶対パス</param>
    /// <param name="fileName">ファイル名</param>
    /// <returns>読み込み結果</returns>
    private string GetTSFile(string path, string fileName)
    {
      var filePath = Path.Combine(path, fileName);

      // ファイル読み込み
      return File.ReadAllText(filePath);
    }

    #endregion


    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      var methodName = nameof(StandardTest);
      
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs",
      @"public class Test
      {
      }");
      CreateCSFile(inputPath, "sub.cs",
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

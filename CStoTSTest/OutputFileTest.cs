using CStoTS;
using System.IO;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("ファイル出力テスト", nameof(OutputFileTest))]
  public class OutputFileTest : TestBase
  {
    #region 専用フィールド・メソッド
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
    #endregion

    #region テスト

    /// <summary>
    /// 標準テスト
    /// </summary>
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
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

    /// <summary>
    /// サブクラス(インスタンスフィールドにクラス定義)のテスト
    /// </summary>
    [Fact(DisplayName = "MultiReferenceTest")]
    public void MultiReferenceTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
      }
      public class MyClass
      {
      }");
      CreateCSFile(inputPath, "sub.cs", 
      @"public class Sub:Test
      {
        private MyClass field;
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test, MyClass } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("  private field: MyClass;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// フォルダ階層違いのテスト
    /// </summary>
    [Fact(DisplayName = "OtherDirectoryTest")]
    public void OtherDirectoryTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "base/test.cs", 
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
      expectedTS.AppendLine("import { Test } from './base/test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// フォルダ階層違い(逆順)のテスト
    /// </summary>
    [Fact(DisplayName = "OtherDirectoryReverseTest")]
    public void OtherDirectoryReverseTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
      }");
      CreateCSFile(inputPath, "sub/sub.cs", 
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "sub/sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from '../test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// フォルダ階層違い(別フォルダ)のテスト
    /// </summary>
    [Fact(DisplayName = "DifferenceReferenceTest")]
    public void DifferenceReferenceTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "base/test.cs", 
      @"public class Test
      {
      }");
      CreateCSFile(inputPath, "sub/sub.cs", 
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "sub/sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from '../base/test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// フォルダ階層違い(フォルダの深さ)のテスト
    /// </summary>
    [Fact(DisplayName = "EachReferenceTest")]
    public void EachReferenceTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "base/test.cs", 
      @"public class Test
      {
      }");
      CreateCSFile(inputPath, "base/sub/sub.cs", 
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "base/sub/sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from '../test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクスのテスト
    /// </summary>
    [Fact(DisplayName = "CollectionTest")]
    public void CollectionTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        private List<string> field1;
        private Dictionary<int,string> field2;
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Dictionary } from './Dictionary';");
      expectedTS.AppendLine("import { List } from './List';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field1: List<string>;");
      expectedTS.AppendLine("  private field2: Dictionary<number, string>;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 内部クラス利用のテスト
    /// </summary>
    [Fact(DisplayName = "UseInnerClassTest")]
    public void UseInnerClassTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        public class Inner {
        }
      }");
      CreateCSFile(inputPath, "other.cs", 
      @"public class Other
      {
        private Test.Inner field;
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "other.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Other {");
      expectedTS.AppendLine("  private field: Test.Inner;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    #endregion
  }
}

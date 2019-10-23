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
    //-----------------------------------------------------

    /// <summary>
    /// ローカルフィールドのテスト
    /// </summary>
    [Fact(DisplayName = "LocalFieldTest")]
    public void LocalFieldTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        public void Method()
        {
          var localVar = ""abc"";
          string localString1 = ""DEF"";
          string localString2;
          int localInt1 = 0;
          int localInt2;
        }
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let localVar: string = \"abc\";");
      expectedTS.AppendLine("    let localString1: string = \"DEF\";");
      expectedTS.AppendLine("    let localString2: string;");
      expectedTS.AppendLine("    let localInt1: number = 0;");
      expectedTS.AppendLine("    let localInt2: number;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ローカルフィールド：クラス生成のテスト
    /// </summary>
    [Fact(DisplayName = "DeclarationClassTest")]
    public void DeclarationClassTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "myclass.cs", 
      @"public class MyClass
      {
        public class Inner {
        }
      }");

      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        public void Method()
        {
          var localMyClass1 = new MyClass();
          MyClass localMyClass2 = new MyClass();
          MyClass localMyClass3;

          var localInner1 = new MyClass.Inner();
          MyClass.Inner localInner2 = new MyClass.Inner();
          MyClass.Inner localInner3;
        }
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { MyClass } from './myclass';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let localMyClass1: MyClass = new MyClass();");
      expectedTS.AppendLine("    let localMyClass2: MyClass = new MyClass();");
      expectedTS.AppendLine("    let localMyClass3: MyClass;");
      expectedTS.AppendLine("    let localInner1: MyClass.Inner = new MyClass.Inner();");
      expectedTS.AppendLine("    let localInner2: MyClass.Inner = new MyClass.Inner();");
      expectedTS.AppendLine("    let localInner3: MyClass.Inner;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// int型のメソッド置き換えのテスト
    /// </summary>
    [Fact(DisplayName = "IntMethodTest")]
    public void IntMethodTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        public int Method1()
        {
          return ""123"".Length;
        }
        public int Method2()
        {
          return int.Parse(""123"");
        }
        public int Method3()
        {
          return Decimal.Parse(""123"");
        }
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method1(): number {");
      expectedTS.AppendLine("    return \"123\".length;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method2(): number {");
      expectedTS.AppendLine("    return Number(\"123\");");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method3(): number {");
      expectedTS.AppendLine("    return Number(\"123\");");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 式のテスト
    /// </summary>
    [Fact(DisplayName = "ExpressionTest")]
    public void ExpressionTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        private int field;
        public void Method()
        {
          this.field = 10*2+1;
          this.field = this.field/2;
        }
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: number;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    this.field = 10 * 2 + 1;");
      expectedTS.AppendLine("    this.field = this.field / 2;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// if：列挙型のテスト
    /// </summary>
    [Fact(DisplayName = "EnumTest")]
    public void EnumTest()
    {
      var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
      var inputPath = GetInputPath(methodName);
      var outputPath = GetOutputPath(methodName);

      // C#ソース作成
      CreateCSFile(inputPath, "test.cs", 
      @"public class Test
      {
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }

        public void Method(){
          var compassDirection = CompassDirection.North;
          if(compassDirection == CompassDirection.North){
          }
        }
      }");

      // 変換
      ConvertTS(false, inputPath, outputPath);

      // 変換確認
      var actualTS = GetTSFile(outputPath, "test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let compassDirection: Test.CompassDirection = Test.CompassDirection.North;");
      expectedTS.AppendLine("    if (compassDirection === Test.CompassDirection.North) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");
      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export enum CompassDirection {");
      expectedTS.AppendLine("    North,");
      expectedTS.AppendLine("    East,");
      expectedTS.AppendLine("    South,");
      expectedTS.AppendLine("    West,");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }


    #endregion
  }
}

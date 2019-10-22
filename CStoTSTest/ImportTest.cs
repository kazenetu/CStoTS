using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("外部ファイル参照のテスト", nameof(ImportTest))]
  public class ImportTest : TestBase
  {
    /// <summary>
    /// 標準テスト
    /// </summary>
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
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
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }
      public class MyClass
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
        private MyClass field;
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
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
      // C#ソース作成
      CreateFileData("base/test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
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
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("sub/sub.cs", string.Empty,
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub/sub.ts");
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
      // C#ソース作成
      CreateFileData("base/test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("sub/sub.cs", string.Empty,
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub/sub.ts");
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
      // C#ソース作成
      CreateFileData("base/test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("base/sub/sub.cs", string.Empty,
      @"public class Sub:Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("base/sub/sub.ts");
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
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private List<string> field1;
        private Dictionary<int,string> field2;
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
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
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public class Inner {
        }
      }");
      CreateFileData("other.cs", string.Empty,
      @"public class Other
      {
        private Test.Inner field;
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("other.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Other {");
      expectedTS.AppendLine("  private field: Test.Inner;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

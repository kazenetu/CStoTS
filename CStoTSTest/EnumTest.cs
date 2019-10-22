using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("列挙型のテスト", nameof(EnumTest))]
  public class EnumTest : TestBase
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
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
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

    /// <summary>
    /// デフォルト設定のテスト
    /// </summary>
    [Fact(DisplayName = "DefaultValueTest")]
    public void DefaultValueTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private enum CompassDirection
        {
          North = 10,
          East,
          South,
          West
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("}");
      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export enum CompassDirection {");
      expectedTS.AppendLine("    North = 10,");
      expectedTS.AppendLine("    East = 11,");
      expectedTS.AppendLine("    South = 12,");
      expectedTS.AppendLine("    West = 13,");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// フィールド定義のテスト
    /// </summary>
    [Fact(DisplayName = "FieldTest")]
    public void FieldTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private CompassDirection CD = CompassDirection.East;
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private CD: Test.CompassDirection = Test.CompassDirection.East;");
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

    /// <summary>
    /// プロパティ設定のテスト
    /// </summary>
    [Fact(DisplayName = "PropertyTest")]
    public void PropertyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private CompassDirection CD{get;} = CompassDirection.East;
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private _CD_: Test.CompassDirection = Test.CompassDirection.East;");
      expectedTS.AppendLine("  private get CD(): Test.CompassDirection {");
      expectedTS.AppendLine("    return this._CD_;");
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

    /// <summary>
    /// 他クラスでの参照のテスト
    /// </summary>
    [Fact(DisplayName = "OtherClassTest")]
    public void OtherClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public enum CompassDirection
        {
          North,
          East,
          South,
          West
        }
      }");
      CreateFileData("other.cs", string.Empty,
      @"public class Ohter
      {
        private Test.CompassDirection CD = Test.CompassDirection.East;
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("other.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from './test';");
      expectedTS.AppendLine();
      expectedTS.AppendLine("export class Ohter {");
      expectedTS.AppendLine("  private CD: Test.CompassDirection = Test.CompassDirection.East;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

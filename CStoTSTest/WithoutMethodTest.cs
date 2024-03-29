﻿using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("メソッド出力除外のテスト", nameof(WithoutMethodTest))]
  public class WithoutMethodTest : TestBase
  {
    /// <summary>
    /// 標準テスト
    /// </summary>
    [Fact(DisplayName = "FieldTest")]
    public void FieldOnlyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
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
      ConvertTS(true);

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: number = 0;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// プロパティのテスト
    /// </summary>
    [Fact(DisplayName = "PropertyTest")]
    public void PropertyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private string field=""ABC"";
        public string Field
        {
          set
          {
            field = value;
          }
          get{
            return field;
          }
        }
        public string Property { set; get; }
        public string Property2 { private set; get; }
        public static string StaticProperty { set; get; }
      }");

      // 変換
      ConvertTS(true);

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: string = \"ABC\";");
      expectedTS.AppendLine("  public set Field(value: string) {");
      expectedTS.AppendLine("    this.field = value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public get Field(): string {");
      expectedTS.AppendLine("    return this.field;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private _Property_: string = \"\";");
      expectedTS.AppendLine("  public set Property(value: string) {");
      expectedTS.AppendLine("    this._Property_ = value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public get Property(): string {");
      expectedTS.AppendLine("    return this._Property_;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private _Property2_: string = \"\";");
      expectedTS.AppendLine("  public set Property2(value: string) {");
      expectedTS.AppendLine("    this._Property2_ = value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public get Property2(): string {");
      expectedTS.AppendLine("    return this._Property2_;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private static _StaticProperty_: string = \"\";");
      expectedTS.AppendLine("  public static set StaticProperty(value: string) {");
      expectedTS.AppendLine("    Test._StaticProperty_ = value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public static get StaticProperty(): string {");
      expectedTS.AppendLine("    return Test._StaticProperty_;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 内部クラス定義のテスト
    /// </summary>
    [Fact(DisplayName = "InnerClassTest")]
    public void InnerClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public class Inner {
        }
        private Test.Inner inner;
      }");

      // 変換
      ConvertTS(true);

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private inner: Test.Inner = new Test.Inner();");
      expectedTS.AppendLine("}");

      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export class Inner {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// コンストラクタのテスト
    /// </summary>
    [Fact(DisplayName = "ConstructorTest")]
    public void ConstructorTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test()
        {
          field=""ABC"";
        }
        private string field;
        public string Field
        {
          set
          {
            field = value;
          }
          get{
            return field;
          }
        }
      }");

      // 変換
      ConvertTS(true);

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // メソッド定義なし
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: string = \"\";");
      expectedTS.AppendLine("  public set Field(value: string) {");
      expectedTS.AppendLine("    this.field = value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public get Field(): string {");
      expectedTS.AppendLine("    return this.field;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// インターフェースのテスト
    /// </summary>
    [Fact(DisplayName = "InterfaceTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test
      {
      }");

      // 変換
      ConvertTS(true);

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

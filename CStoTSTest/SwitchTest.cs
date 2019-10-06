using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("複数条件分岐のテスト", nameof(SwitchTest))]
  public class SwitchTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var val = 1;
          switch(val)
          {
            case 1:
              break;
          }
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let val: number = 1;");
      expectedTS.AppendLine("    switch (val) {");
      expectedTS.AppendLine("      case 1:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "ExistsDefaultTest")]
    public void ExistsDefaultTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var val = 1;
          switch(val)
          {
            case 1:
              break;
            default:
              break;
          }
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let val: number = 1;");
      expectedTS.AppendLine("    switch (val) {");
      expectedTS.AppendLine("      case 1:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("      default:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "MultiCaseTest")]
    public void MultiCaseTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var val = 1;
          switch(val)
          {
            case 1:
            case 2:
              break;
            default:
              break;
          }
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let val: number = 1;");
      expectedTS.AppendLine("    switch (val) {");
      expectedTS.AppendLine("      case 1:");
      expectedTS.AppendLine("      case 2:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("      default:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "TypeCaseTest")]
    public void TypeCaseTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          object val = 1;
          switch (val)
          {
            case int i:
              break;
            case string s:
              break;
            case DateTime d:
              break;
          }
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let val: any = 1;");
      expectedTS.AppendLine("    if (typeof val === \"number\") {");
      expectedTS.AppendLine("      let i: number = val;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof val === \"string\") {");
      expectedTS.AppendLine("      let s: string = val;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (val instanceof Date) {");
      expectedTS.AppendLine("      let d: Date = val;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "InstancePropertyTest")]
    public void InstancePropertyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private int Val{get;}= 1;
        public void Method(){
          switch(Val)
          {
            case 1:
              break;
            default:
              break;
          }
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
      expectedTS.AppendLine("  private _Val_: number = 1;");
      expectedTS.AppendLine("  private get Val(): number {");
      expectedTS.AppendLine("    return this._Val_;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    switch (this.Val) {");
      expectedTS.AppendLine("      case 1:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("      default:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "InstanceMethodTest")]
    public void InstanceMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private int GetValue()
        {
          return 1;
        }
        public void Method(){
          switch(GetValue())
          {
            case 1:
              break;
            default:
              break;
          }
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
      expectedTS.AppendLine("  private GetValue(): number {");
      expectedTS.AppendLine("    return 1;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    switch (this.GetValue()) {");
      expectedTS.AppendLine("      case 1:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("      default:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "EnumTest")]
    public void EnumTest()
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

        public void Method(){
          var compassDirection = CompassDirection.North;
          switch(compassDirection)
          {
            case CompassDirection.North:
              break;
            case CompassDirection.East:
              break;
          }
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let compassDirection: Test.CompassDirection = Test.CompassDirection.North;");
      expectedTS.AppendLine("    switch (compassDirection) {");
      expectedTS.AppendLine("      case Test.CompassDirection.North:");
      expectedTS.AppendLine("        break;");
      expectedTS.AppendLine("      case Test.CompassDirection.East:");
      expectedTS.AppendLine("        break;");
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

  }
}

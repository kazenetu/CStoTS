using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("プロパティのテスト", nameof(PropertyTest))]
  public class PropertyTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
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
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: string;");
      expectedTS.AppendLine("  public set Field(value: string) {");
      expectedTS.AppendLine("    this.field = value;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("  public get Field(): string {");
      expectedTS.AppendLine("    return this.field;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "SetGetTest")]
    public void SetGetTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public string Field { set; get; }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private _Field_: string;");
      expectedTS.AppendLine("  public set Field(value: string) {");
      expectedTS.AppendLine("    this._Field_ = value;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("  public get Field(): string {");
      expectedTS.AppendLine("    return this._Field_;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }
    
    [Fact(DisplayName = "EachModifiyTest")]
    public void EachModifiyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public string Field { private set; get; }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private _Field_: string;");
      expectedTS.AppendLine("  public set Field(value: string) {");
      expectedTS.AppendLine("    this._Field_ = value;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("  public get Field(): string {");
      expectedTS.AppendLine("    return this._Field_;");
      expectedTS.AppendLine("  };");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

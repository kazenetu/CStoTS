using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("C#の特定メソッドをTypeScriptのメソッドに置き換えのテスト", nameof(ReplaceTSMethodTest))]
  public class ReplaceTSMethodTest : TestBase
  {
    [Fact(DisplayName = "StringMethodTest")]
    public void StringMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public string Method1()
        {
          return String.Empty;
        }
        public string Method2()
        {
          return string.Empty;
        }
        public string Method3()
        {
          return (10).ToString();
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method1(): string {");
      expectedTS.AppendLine("    return '';");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method2(): string {");
      expectedTS.AppendLine("    return '';");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method3(): string {");
      expectedTS.AppendLine("    return (new List<int>()).ToString();");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "IntMethodTest")]
    public void IntMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
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
      }");
      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method1(): number {");
      expectedTS.AppendLine("    return \"123\".length;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method2(): number {");
      expectedTS.AppendLine("    return Number(\"123\");");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}

using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("反復処理(foreach)のテスト", nameof(ForeachTest))]
  public class ForeachTest : TestBase
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
        public void Method(){
          int[] target = { 1, 2, 3 };
          foreach (var element in target)
          {
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
      expectedTS.AppendLine("    let target: number[] = [1, 2, 3];");
      expectedTS.AppendLine("    for (let element of target) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクス(List)のテスト
    /// </summary>
    [Fact(DisplayName = "GenericsListTest")]
    public void GenericsListTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var target = new List<string>();
          target.Add(""A"");
          target.Add(""B"");
          target.Add(""C"");
          foreach (var element in target)
          {
          }
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定(List.tsは別途出力されるArrayラッパークラス)
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { List } from './List';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let target: List<string> = new List<string>();");
      expectedTS.AppendLine("    target.Add(\"A\");");
      expectedTS.AppendLine("    target.Add(\"B\");");
      expectedTS.AppendLine("    target.Add(\"C\");");
      expectedTS.AppendLine("    for (let element of target) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクス(Dictionary)のテスト
    /// </summary>
    [Fact(DisplayName = "GenericsDictionaryTest")]
    public void GenericsDictionaryTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var target = new Dictionary<int,string>();
          target.Add(1, ""A"");
          target.Add(2, ""B"");
          target.Add(3, ""C"");
          foreach (var element in target)
          {
            var keyInt = element.Key;
            var valueString = element.Value;
          }
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定(Dictionary.tsは別途出力されるMapラッパークラス)
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Dictionary } from './Dictionary';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let target: Dictionary<number, string> = new Dictionary<number, string>();");
      expectedTS.AppendLine("    target.Add(1, \"A\");");
      expectedTS.AppendLine("    target.Add(2, \"B\");");
      expectedTS.AppendLine("    target.Add(3, \"C\");");
      expectedTS.AppendLine("    for (let [k, v] of target) {");
      expectedTS.AppendLine("      let element = { \"Key\": k, \"Value\": v };");
      expectedTS.AppendLine("      let keyInt: number = element.Key;");
      expectedTS.AppendLine("      let valueString: string = element.Value;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
